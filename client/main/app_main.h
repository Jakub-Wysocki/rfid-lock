#ifndef APP_MAIN_H
#define APP_MAIN_H


/* SPI CONFIG*/
#define PN532_SCK 19
#define PN532_MOSI 23
#define PN532_SS 22
#define PN532_MISO 25

static const char *MQTT_TAG = "MQTT";
static const char *TAG_RFID = "RFID";

struct card_uid_struct {
    char *card_uid;
};

#endif //APP_MAIN_H