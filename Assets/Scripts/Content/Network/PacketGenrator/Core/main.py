from HyperBot import HyperSlackBot_Slave
import ssl
print(ssl.OPENSSL_VERSION)
print(ssl.OPENSSL_VERSION_INFO)
evt_token = "xapp-1-A06LJ74LU5S-6692869741925-a0598f26ce6c33de2fb79fddbcd7b4f22cf0a3a9d50aa7898c8cd11e758bb065"
app_token = "xoxb-5801627857079-6708456657473-34V8cEvy0scJ7a7VItHqeoJK"

if __name__ == "__main__":
    print("starting...")
    bot = HyperSlackBot_Slave(app_token, evt_token)
    bot.start()
    
    print(f"exit with error code : {-1}")