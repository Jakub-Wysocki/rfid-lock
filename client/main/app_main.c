#include <stdio.h>
#include <stdint.h>
#include <stddef.h>
#include <string.h>
#include "esp_wifi.h"
#include "esp_system.h"
#include "nvs_flash.h"
#include "esp_event.h"
#include "esp_netif.h"
#include "protocol_examples_common.h"

#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "freertos/semphr.h"
#include "freertos/queue.h"
#include "freertos/event_groups.h"

#include "lwip/sockets.h"
#include "lwip/dns.h"
#include "lwip/netdb.h"

#include "esp_log.h"
#include "mqtt_client.h"

#include "app_main.h"
#include "pn532.h"
#include "driver/gpio.h"
#include "ll.h"

struct card_uid_struct
{
    char card_uid[32];
};

static pn532_t nfc;
struct card_uid_struct *cards = NULL;
esp_mqtt_client_handle_t client;

static esp_err_t mqtt_event_handler_cb(esp_mqtt_event_handle_t event)
{
    esp_mqtt_client_handle_t client = event->client;
    int msg_id;
    switch (event->event_id)
    {
    case MQTT_EVENT_CONNECTED:
        ESP_LOGI(MQTT_TAG, "MQTT_EVENT_CONNECTED");
        msg_id = esp_mqtt_client_subscribe(client, "/rfid/add", 0);
        msg_id = esp_mqtt_client_subscribe(client, "/rfid/remove", 0);
        msg_id = esp_mqtt_client_subscribe(client, "/rfid/list", 0);
        break;
    case MQTT_EVENT_DISCONNECTED:
        ESP_LOGI(MQTT_TAG, "MQTT_EVENT_DISCONNECTED");
        break;
    case MQTT_EVENT_SUBSCRIBED:
        ESP_LOGI(MQTT_TAG, "MQTT_EVENT_SUBSCRIBED, msg_id=%d", event->msg_id);
        break;
    case MQTT_EVENT_UNSUBSCRIBED:
        ESP_LOGI(MQTT_TAG, "MQTT_EVENT_UNSUBSCRIBED, msg_id=%d", event->msg_id);
        break;
    case MQTT_EVENT_PUBLISHED:
        ESP_LOGI(MQTT_TAG, "MQTT_EVENT_PUBLISHED, msg_id=%d", event->msg_id);
        break;
    case MQTT_EVENT_DATA:

        if (strncmp(event->topic, "/rfid/add", 7) == 0)
        {
            char tmp_str[32];

            for(int i = 0; i < event->data_len; i++)
                tmp_str[i] = event->data[i];

            tmp_str[event->data_len + 1] = 0;

            cards = ll_new(cards);
            strcpy(cards->card_uid, tmp_str);
            
            printf("ADDED DEVICE %s\r\n", cards->card_uid);
        }
        else if (strncmp(event->topic, "/rfid/remove", 7) == 0)
        {
            if(cards == NULL)
            {
                msg_id = esp_mqtt_client_publish(client, "/topic/remove", "NO DEVICES", 0, 1, 0);
                break;
            }

            int flag = 0;
            char tmp_str[32];

            for(int i = 0; i < event->data_len; i++)
                tmp_str[i] = event->data[i];

            tmp_str[event->data_len + 1] = 0;


            printf("RECEIVED DATA=%s\r\n", tmp_str);

            ll_foreach(cards, card)
            {
                printf("COMPARED DATA: %s\r\n", card->card_uid);

                if (strncmp(card->card_uid, event->data, 3) == 0)
                {
                    cards = ll_pop(cards);
                    flag++;
                    msg_id = esp_mqtt_client_publish(client, "/topic/remove", "REMOVED", 0, 1, 0);
                }
            }

            if (flag == 0)
                msg_id = esp_mqtt_client_publish(client, "/topic/remove", "NO DEVICE", 0, 1, 0);
        }
        else if (strncmp(event->topic, "/rfid/list", 7) == 0)
        {
            if(cards == NULL)
            {
                msg_id = esp_mqtt_client_publish(client, "/topic/remove", "NO DEVICES", 0, 1, 0);
                break;
            }

            ll_foreach(cards, card)
            {
                printf("CARD = %s\r\n", card->card_uid);
                msg_id = esp_mqtt_client_publish(client, "/topic/list", card->card_uid, 0, 1, 0);
                
            }
            
        }
        else
        {
            ESP_LOGI(MQTT_TAG, "WRONG TOPIC");
            printf("TOPIC=%.*s\r\n", event->topic_len, event->topic);
            printf("DATA=%.*s\r\n", event->data_len, event->data);
        }

        break;
    case MQTT_EVENT_ERROR:
        ESP_LOGI(MQTT_TAG, "MQTT_EVENT_ERROR");
        break;
    default:
        ESP_LOGI(MQTT_TAG, "Other event id:%d", event->event_id);
        break;
    }
    return ESP_OK;
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

            //access_attempt(str_uid);

        }
        else
        {
            // PN532 probably timed out waiting for a card
            ESP_LOGI(TAG_RFID, "Timed out waiting for a card");
        }
    }
}

/*
//function handles access attempt. 
void access_attempt(char* str_uid)
{
    char result[160];

    ll_foreach(cards, card)
    {
        int flag = 0;
        if(strcmp(card->card_uid, event->data) == 0)
            {
                result = "Access granted to: "
                flag++;
                msg_id = esp_mqtt_client_publish(client, "/topic/log", strcat(result, str_uid) , 0, 1, 0);
                ESP_LOGI(TAG, "sent publish successful, msg_id=%d", msg_id);
            }

        if(flag == 0)
        {
            result = "Access denied to: "
            msg_id = esp_mqtt_client_publish(client, "/topic/log", strcat(result, str_uid) , 0, 1, 0);
            ESP_LOGI(TAG, "sent publish successful, msg_id=%d", msg_id);
        }
        else
        {
            ESP_LOGI(TAG, "Hej debug patrz na to! Access_attempt msg_id=%d", msg_id);
        }

    }

}*/

static void mqtt_event_handler(void *handler_args, esp_event_base_t base, int32_t event_id, void *event_data)
{
    ESP_LOGD(MQTT_TAG, "Event dispatched from event loop base=%s, event_id=%d", base, event_id);
    mqtt_event_handler_cb(event_data);
}

static void mqtt_app_start(void)
{
    esp_mqtt_client_config_t mqtt_cfg = {
        .uri = CONFIG_BROKER_URL,
    };

    client = esp_mqtt_client_init(&mqtt_cfg);
    esp_mqtt_client_register_event(client, ESP_EVENT_ANY_ID, mqtt_event_handler, client);
    esp_mqtt_client_start(client);
}

void app_main(void)
{

    ESP_LOGI(MQTT_TAG, "[APP] Startup..");
    ESP_LOGI(MQTT_TAG, "[APP] Free memory: %d bytes", esp_get_free_heap_size());
    ESP_LOGI(MQTT_TAG, "[APP] IDF version: %s", esp_get_idf_version());

    esp_log_level_set("*", ESP_LOG_INFO);
    esp_log_level_set("MQTT_CLIENT", ESP_LOG_VERBOSE);
    esp_log_level_set("MQTT_EXAMPLE", ESP_LOG_VERBOSE);
    esp_log_level_set("TRANSPORT_TCP", ESP_LOG_VERBOSE);
    esp_log_level_set("TRANSPORT_SSL", ESP_LOG_VERBOSE);
    esp_log_level_set("TRANSPORT", ESP_LOG_VERBOSE);
    esp_log_level_set("OUTBOX", ESP_LOG_VERBOSE);

    ESP_ERROR_CHECK(nvs_flash_init());
    ESP_ERROR_CHECK(esp_netif_init());
    ESP_ERROR_CHECK(esp_event_loop_create_default());

    ESP_ERROR_CHECK(example_connect());

    mqtt_app_start(); //there could be an error with nfc task priority
    //xTaskCreate(&mqtt_app_start, "mqtt", 4096, NULL, 1, NULL);
    xTaskCreate(&nfc_task, "nfc_task", 4096, NULL, 1, NULL);
}
