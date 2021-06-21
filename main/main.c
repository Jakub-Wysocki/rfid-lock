#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include "freertos/FreeRTOS.h"
#include "freertos/event_groups.h"
#include "freertos/task.h"
#include "esp_system.h"
#include "esp_wifi.h"
#include "esp_spi_flash.h"
#include "esp_event.h"
#include "driver/gpio.h"

#include "sdkconfig.h"
#include "esp_log.h"
#include <esp_log_internal.h>
#include "nvs_flash.h"

#include "lwip/err.h"
#include "lwip/sockets.h"
#include "lwip/sys.h"
#include "lwip/netdb.h"
#include "lwip/dns.h"

#include "pn532.h"

/*----------------------*/

#define EXAMPLE_ESP_WIFI_SSID 
#define EXAMPLE_ESP_WIFI_PASS 
#define EXAMPLE_ESP_MAXIMUM_RETRY 1

/* This is the config of the TCP Client */
#define MESSAGE "1"
#define TCPServerIP 

/* FreeRTOS event group to signal when we are connected*/
static EventGroupHandle_t s_wifi_event_group;

/* The event group allows multiple bits for each event, but we only care about two events:
 * - we are connected to the AP with an IP
 * - we failed to connect after the maximum amount of retries */
#define WIFI_CONNECTED_BIT BIT0
#define WIFI_FAIL_BIT      BIT1

/* SPI CONFIG*/
#define PN532_SCK 19
#define PN532_MOSI 23
#define PN532_SS 22
#define PN532_MISO 25

/* GPIO PINS*/
#define GPIO_BUZZER 27
#define GPIO_RED_LED 14

static const char *TAG_WIFI = "WiFi";
static int s_retry_num = 0;
static const char *TAG_RFID = "RFID";
static pn532_t nfc;

static void event_handler(void* arg, esp_event_base_t event_base, int32_t event_id, void* event_data)
{
    if (event_base == WIFI_EVENT && event_id == WIFI_EVENT_STA_START) {
        esp_wifi_connect();
    } else if (event_base == WIFI_EVENT && event_id == WIFI_EVENT_STA_DISCONNECTED) {
        if (s_retry_num < EXAMPLE_ESP_MAXIMUM_RETRY) {
            esp_wifi_connect();
            s_retry_num++;
            ESP_LOGI(TAG_WIFI, "retry to connect to the AP");
        } else {
            xEventGroupSetBits(s_wifi_event_group, WIFI_FAIL_BIT);
        }
        ESP_LOGI(TAG_WIFI,"connect to the AP fail");
    } else if (event_base == IP_EVENT && event_id == IP_EVENT_STA_GOT_IP) {
        ip_event_got_ip_t* event = (ip_event_got_ip_t*) event_data;
        ESP_LOGI(TAG_WIFI, "got ip:" IPSTR, IP2STR(&event->ip_info.ip));
        s_retry_num = 0;
        xEventGroupSetBits(s_wifi_event_group, WIFI_CONNECTED_BIT);
    }
}

void wifi_init_sta(void)
{
    s_wifi_event_group = xEventGroupCreate();

    ESP_ERROR_CHECK(esp_netif_init());

    ESP_ERROR_CHECK(esp_event_loop_create_default());
    esp_netif_create_default_wifi_sta();

    wifi_init_config_t cfg = WIFI_INIT_CONFIG_DEFAULT();
    ESP_ERROR_CHECK(esp_wifi_init(&cfg));

    ESP_ERROR_CHECK(esp_event_handler_register(WIFI_EVENT, ESP_EVENT_ANY_ID, &event_handler, NULL));
    ESP_ERROR_CHECK(esp_event_handler_register(IP_EVENT, IP_EVENT_STA_GOT_IP, &event_handler, NULL));


 
    wifi_config_t wifi_config = {
        .sta = {
            .ssid = EXAMPLE_ESP_WIFI_SSID,
            .password = EXAMPLE_ESP_WIFI_PASS,
            /* Setting a password implies station will connect to all security modes including WEP/WPA.
             * However these modes are deprecated and not advisable to be used. Incase your Access point
             * doesn't support WPA2, these mode can be enabled by commenting below line */
	        //.threshold.authmode = WIFI_AUTH_WPA2_PSK,
            .pmf_cfg = {
                .capable = true,
                .required = false
            },
        },
    }; 

    ESP_ERROR_CHECK(esp_wifi_set_mode(WIFI_MODE_STA) );
    ESP_ERROR_CHECK(esp_wifi_set_config(ESP_IF_WIFI_STA, &wifi_config) );
    ESP_ERROR_CHECK(esp_wifi_start() );
    
    ESP_LOGI(TAG_WIFI, "wifi_init_sta finished.");

    /* Waiting until either the connection is established (WIFI_CONNECTED_BIT) or connection failed for the maximum
     * number of re-tries (WIFI_FAIL_BIT). The bits are set by event_handler() (see above) */
    EventBits_t bits = xEventGroupWaitBits(s_wifi_event_group,
            WIFI_CONNECTED_BIT | WIFI_FAIL_BIT,
            pdFALSE,
            pdFALSE,
            portMAX_DELAY);

    /* xEventGroupWaitBits() returns the bits before the call returned, hence we can test which event actually
     * happened. */
    if (bits & WIFI_CONNECTED_BIT) {
        ESP_LOGI(TAG_WIFI, "connected to ap SSID:%s password:%s",
                 EXAMPLE_ESP_WIFI_SSID, EXAMPLE_ESP_WIFI_PASS);
    } else if (bits & WIFI_FAIL_BIT) {
        ESP_LOGI(TAG_WIFI, "Failed to connect to SSID:%s, password:%s",
                 EXAMPLE_ESP_WIFI_SSID, EXAMPLE_ESP_WIFI_PASS);
    } else {
        ESP_LOGE(TAG_WIFI, "UNEXPECTED EVENT");
    }

    ESP_ERROR_CHECK(esp_event_handler_unregister(IP_EVENT, IP_EVENT_STA_GOT_IP, &event_handler));
    ESP_ERROR_CHECK(esp_event_handler_unregister(WIFI_EVENT, ESP_EVENT_ANY_ID, &event_handler));
    vEventGroupDelete(s_wifi_event_group);
}

void beep(int result)
{

    if(!result)
    {   
        gpio_pad_select_gpio(GPIO_BUZZER);
        gpio_set_direction(GPIO_BUZZER, GPIO_MODE_OUTPUT);
        gpio_set_level(GPIO_BUZZER, 1);
        vTaskDelay(2000 / portTICK_PERIOD_MS);
        gpio_set_level(GPIO_BUZZER, 0);
    }
    else
    {

        gpio_pad_select_gpio(GPIO_RED_LED);
        gpio_set_direction(GPIO_RED_LED, GPIO_MODE_OUTPUT);
        gpio_set_level(GPIO_RED_LED, 1);
        vTaskDelay(3000 / portTICK_PERIOD_MS);
        gpio_set_level(GPIO_RED_LED, 0);

    }

}

void tcp_client(char* data){
    ESP_LOGI(TAG_WIFI,"tcp_client task started \n");
    struct sockaddr_in tcpServerAddr;
    tcpServerAddr.sin_addr.s_addr = inet_addr(TCPServerIP);
    tcpServerAddr.sin_family = AF_INET;
    tcpServerAddr.sin_port = htons( 50000 );
    int s, r;
    char recv_buf[64], result[64]; // for reading purposes only */
    while(1){
        
        s = socket(AF_INET, SOCK_STREAM, 0);
        if(s < 0) {
            ESP_LOGE(TAG_WIFI, "... Failed to allocate socket.\n");
            vTaskDelay(1000 / portTICK_PERIOD_MS);
            continue;
        }
        ESP_LOGI(TAG_WIFI, "... allocated socket\n");
         if(connect(s, (struct sockaddr *)&tcpServerAddr, sizeof(tcpServerAddr)) != 0) {
            ESP_LOGE(TAG_WIFI, "... socket connect failed errno=%d \n", errno);
            close(s);
            vTaskDelay(4000 / portTICK_PERIOD_MS);
            continue;
        }
        ESP_LOGI(TAG_WIFI, "... connected \n");
        if( write(s , data , strlen(data)) < 0)
        {
            ESP_LOGE(TAG_WIFI, "... Send failed \n");
            close(s);
            vTaskDelay(4000 / portTICK_PERIOD_MS);
            continue;
        }
        ESP_LOGI(TAG_WIFI, "... socket send success\n");
        do {
            bzero(recv_buf, sizeof(recv_buf));
            r = read(s, recv_buf, sizeof(recv_buf)-1);
            for(int i = 0; i < r; i++) {
                putchar(recv_buf[i]);
                
                result[i] = recv_buf[i];
            }
            printf("\n");


        } while(r > 0);
        ESP_LOGI(TAG_WIFI, "... done reading from socket. Last read return=%d errno=%d\r\n", r, errno);
        ESP_LOGI(TAG_WIFI, "... Result: %s\n", result);
        close(s);
        break;
    
    }

    beep(strncmp(result, "Access granted", 8));
    ESP_LOGI(TAG_WIFI, "...tcp_client task closed\n");
}

void nfc_task(void *pvParameter)
{
    pn532_spi_init(&nfc, PN532_SCK, PN532_MISO, PN532_MOSI, PN532_SS);
    pn532_begin(&nfc);

    uint32_t versiondata = pn532_getFirmwareVersion(&nfc);
    if (!versiondata)
    {
        ESP_LOGI(TAG_RFID, "Didn't find PN53x board");
        while (1)
        {
            vTaskDelay(1000 / portTICK_RATE_MS);
        }
    }
    // Got ok data, print it out!
    ESP_LOGI(TAG_RFID, "Found chip PN5 %x", (versiondata >> 24) & 0xFF);
    ESP_LOGI(TAG_RFID, "Firmware ver. %d.%d", (versiondata >> 16) & 0xFF, (versiondata >> 8) & 0xFF);

    // configure board to read RFID tags
    pn532_SAMConfig(&nfc);

    ESP_LOGI(TAG_RFID, "Waiting for an ISO14443A Card ...");

    while (1)
    {
        uint8_t success;
        uint8_t uid[] = {0, 0, 0, 0, 0, 0, 0}; // Buffer to store the returned UID
        uint8_t uidLength;                     // Length of the UID (4 or 7 bytes depending on ISO14443A card type)

        // Wait for an ISO14443A type cards (Mifare, etc.).  When one is found
        // 'uid' will be populated with the UID, and uidLength will indicate
        // if the uid is 4 bytes (Mifare Classic) or 7 bytes (Mifare Ultralight)
        success = pn532_readPassiveTargetID(&nfc, PN532_MIFARE_ISO14443A, uid, &uidLength, 0);

        if (success)
        {
            // Display some basic information about the card
            ESP_LOGI(TAG_RFID, "Found an ISO14443A card");
            ESP_LOGI(TAG_RFID, "UID Length: %d bytes", uidLength);
            ESP_LOGI(TAG_RFID, "UID Value:");
            esp_log_buffer_hexdump_internal(TAG_RFID, uid, uidLength, ESP_LOG_INFO);   
            vTaskDelay(1000 / portTICK_RATE_MS);

            char str_uid[128];
            int index = 0;
            for (int i=0; i < 5; i++)
                index += sprintf(&str_uid[index], "%d", uid[i]);

            tcp_client(str_uid);
              
        }
        else
        {
            // PN532 probably timed out waiting for a card
            ESP_LOGI(TAG_RFID, "Timed out waiting for a card");
        }
    }
}


void app_main(void)
{
    
    //Initialize NVS
    esp_err_t ret = nvs_flash_init();
    if (ret == ESP_ERR_NVS_NO_FREE_PAGES || ret == ESP_ERR_NVS_NEW_VERSION_FOUND) {
      ESP_ERROR_CHECK(nvs_flash_erase());
      ret = nvs_flash_init();
    }
    ESP_ERROR_CHECK(ret);

    ESP_LOGI(TAG_WIFI, "ESP_WIFI_MODE_STA");
    wifi_init_sta();

    xTaskCreate(&nfc_task, "nfc_task", 4096, NULL, 4, NULL);

}
