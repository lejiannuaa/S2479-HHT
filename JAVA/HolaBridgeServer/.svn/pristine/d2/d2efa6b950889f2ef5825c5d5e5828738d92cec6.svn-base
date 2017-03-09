package com.hola.bs.print.rmi;

import java.io.IOException;
import java.net.Socket;
import java.rmi.server.RMIClientSocketFactory;

/**
 * 自定义socket连接，设置超时时间
 * @author S2139
 * 2012 Jun 12, 2012 2:01:32 PM 
 */
public class RMICustomClientSocketFactory implements RMIClientSocketFactory {

	private int timeout;
	
	public int getTimeout() {
		return timeout;
	}

	public void setTimeout(int timeout) {
		this.timeout = timeout;
	}


	public Socket createSocket(String host, int port) throws IOException {
		// TODO Auto-generated method stub
		Socket socket = new Socket(host,port);  
        socket.setSoTimeout(timeout);  
        return socket;  
	}

}
