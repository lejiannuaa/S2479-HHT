package com.hola.bs.pool;

import java.io.IOException;

import com.hola.bs.socket.TCPClient;



public class test {
	public static void main(String[] args) throws InterruptedException{
			TCPClient t;
			try {
				t = new TCPClient("10.130.1.44",1978);
				String msg = "request=501;usr=SOM;op=04;sto=13105";
				int length = msg.length();
				t.sendMsg("XXXX0000"+length+"00000000000000000000000000000000");
				Thread.sleep(500);
			    t.sendMsg("request=501;usr=SOM;op=04;sto=13105");
			    Thread.sleep(2000);
			    t.end();
			    
			} catch (IOException e) {
				e.printStackTrace();
			}
	}
}
