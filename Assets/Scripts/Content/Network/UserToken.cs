using System.Net.Sockets;

class UserToken
{
    void on_new_client(Socket client_socket, object token)
    {
/*        // �ÿ��� �ϳ� ������ ����Ѵ�.
        SocketAsyncEventArgs receive_args = this.receive_event_args_pool.Pop();
        SocketAsyncEventArgs send_args = this.send_event_args_pool.Pop();
        // SocketAsyncEventArgs�� ������ �� ����� �ξ��� CUserToken�� �����ͼ�
        // �ݹ� �޼����� �Ķ���ͷ� �Ѱ��ش�.
        if (this.session_created_callback != null)
        {
            UserToken user_token = receive_args.UserToken as UserToken;
            this.session_created_callback(user_token);
        }

        // Ŭ���̾�Ʈ�κ��� �����͸� ������ �غ� �Ѵ�.
        begin_receive(client_socket, receive_args, send_args);*/
    }
}
