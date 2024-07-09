from HyperBot import HyperSlackBot_Slave
import ssl
print(ssl.OPENSSL_VERSION)
print(ssl.OPENSSL_VERSION_INFO)
# applicator
evt_token = "xapp-1-A07AAA54DK7-7353249768306-b0916ea1d3b94d7f7542a275fbbb51efb1cf4aa8fd80bee8ba528ef2f5b021d0"
app_token = "xoxb-5801627857079-7338700948199-ZcTJjtmihEq5CtCl01yM0OCU"

if __name__ == "__main__":
    print("starting...")
    bot = HyperSlackBot_Slave(app_token, evt_token)
    bot.start_at_server()
    
    print(f"exit with error code : {-1}")