package com.hola.bs.pool;

import java.nio.ByteBuffer;
import java.nio.channels.SocketChannel;

public class testThread implements Runnable{

	private SocketChannel socketChannel;
	private String message;
	
	public testThread(SocketChannel socketChannel,String message){
		this.socketChannel = socketChannel;
		this.message = message;
		new Thread(this).start();
	}
	
	public void run() {
		ByteBuffer writeBuffer;
		try {
			System.out.println("=sendMsg=");
			writeBuffer = ByteBuffer.wrap(message.getBytes("UTF-8"));
			socketChannel.write(writeBuffer);
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

}
