package com.hola.bs.socket;

import java.io.IOException;
import java.nio.ByteBuffer;

import org.xsocket.connection.BlockingConnection;
import org.xsocket.connection.IBlockingConnection;
import org.xsocket.connection.INonBlockingConnection;
import org.xsocket.connection.NonBlockingConnection;

/**
 * 客户端接收服务端信息 IBlockingConnection：这个的话就是不支持事件回调处理机制的！
 * INonBlockingConnection:这个连接支持回调机制
 * 
 * 非阻塞的客户端是能够支持事件处理的方法的。即如果从网络通道中没有取到想要的数据就会自动退出程序
 * @author S1608
 *
 */
public class XSocketClient {

	private static final int PORT = 1978;
	private static final String server_ip = "192.168.1.126";

	public void sendMsg(String msg) throws Exception{
		// 采用非阻塞式的连接
		INonBlockingConnection nbc = new NonBlockingConnection(server_ip, PORT, new XClientHandler());

		// 采用阻塞式的连接
		// IBlockingConnection bc = new BlockingConnection("localhost", PORT);
		// 一个非阻塞的连接是很容易就变成一个阻塞连接
		IBlockingConnection bc = new BlockingConnection(nbc);
		// 设置编码格式
		bc.setEncoding("UTF-8");
		// 设置是否自动清空缓存
		bc.setAutoflush(true);
		// 向服务端写数据信息
//		for (int i = 0; i < 100; i++) {
//			bc.write(" client | i |love |china !..." + i);
//		}
		
//		String msg = "莎啦啦啦！";
		ByteBuffer bb = ByteBuffer.wrap(msg.getBytes("UTF-8"));
		bc.write(bb);
		
		// 向客户端读取数据的信息
		byte[] byteBuffers = bc.readBytesByDelimiter("|", "UTF-8");
		// 打印服务器端信息
		System.out.println(new String(byteBuffers));
		// 将信息清除缓存，写入服务器端
		bc.flush();
		bc.close();
	}
	
	public static void main(String[] args) throws IOException {
		XSocketClient client = new XSocketClient();
		try{
			client.sendMsg("request=login;usr=S2132;pwd=123456;times=1;sn=03FF2DxdAc");
//			client.sendMsg("request=004;usr=S2132;op=03;bc=10000");
			
//			client.sendMsg("request=004;usr=T2215;op=01;bc=;from=20110103;to=20121011;state=;opusr=");
//			client.sendMsg("request=004;usr=S2136;op=02;bc=466138");
//			client.sendMsg("request=101;usr=S1777;op=02;bc=000889");
//			client.sendMsg("request=101;usr=S1777;op=02;bc=000889");		
//			client.sendMsg("request=login;usr=T2215;pwd=123456");
//			client.sendMsg("request=002;usr=T2215;op=04;bc=0000032667");
//			client.sendMsg("request=201;usr=T2215;op=01;from=20120101;to=20120430;bcfrom=000001;bcto=99999;boxbcfrom=;boxbcto=;type=TRF;state=Y;frml=HHT");
//			client.sendMsg("request=202;usr=S2135;op=03;bc=87955;type=RTV");

		}catch(Exception e){
			e.printStackTrace();
		}

	}

}
