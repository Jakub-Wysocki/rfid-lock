#ifndef APP_MAIN_H
#define APP_MAIN_H


#define EXAMPLE_ESP_WIFI_SSID ""
#define EXAMPLE_ESP_WIFI_PASS "" 
#define EXAMPLE_ESP_MAXIMUM_RETRY 1

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

static const char *MQTT_TAG = "MQTT";

static const char *TAG_WIFI = "WiFi";
static const char *TAG_RFID = "RFID";
static int s_retry_num = 0;



#endif //APP_MAIN_H