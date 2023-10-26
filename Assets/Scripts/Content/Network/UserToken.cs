using System.Net.Sockets;

class UserToken
{
    void on_new_client(Socket client_socket, object token)
    {
/*        // 플에서 하나 꺼내와 사용한다.
        SocketAsyncEventArgs receive_args = this.receive_event_args_pool.Pop();
        SocketAsyncEventArgs send_args = this.send_event_args_pool.Pop();
        // SocketAsyncEventArgs를 생성할 때 만들어 두었던 CUserToken을 꺼내와서
        // 콜백 메서드의 파라미터로 넘겨준다.
        if (this.session_created_callback != null)
        {
            UserToken user_token = receive_args.UserToken as UserToken;
            this.session_created_callback(user_token);
        }

        // 클라이언트로부터 데이터를 수신할 준비를 한다.
        begin_receive(client_socket, receive_args, send_args);*/
    }
}
