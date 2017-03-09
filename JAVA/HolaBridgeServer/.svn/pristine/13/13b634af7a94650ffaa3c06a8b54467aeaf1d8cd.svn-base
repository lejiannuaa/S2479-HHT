package com.hola.bs.socket;

import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.channels.SelectionKey;
import java.nio.channels.ServerSocketChannel;
import java.nio.channels.SocketChannel;
import java.nio.charset.Charset;

import org.springframework.beans.factory.annotation.Autowired;

import com.hola.bs.core.Command;
import com.hola.bs.core.exception.UserTimeoutException;


/**
 * TCPProtocol的实现类
 * 
 */
public class TCPProtocolImpl implements TCPProtocol {
	
	private int bufferSize;
	

	/**
	 * 命令处理装置
	 */
	@Autowired(required=true)
	private Command command;
	
	public Command getCommand() {
		return command;
	}
	
	public void setCommand(Command command) {
		this.command = command;
	}

	public void handleAccepted(SelectionKey key) throws IOException {
		// 当有新连接请求进入时，开启一个新的非阻塞套接字通道，并注册到选择器上
		// 可选操作：为其加上一个byte缓存附件
		SocketChannel clientChannel = ((ServerSocketChannel) key.channel()).accept();
		clientChannel.configureBlocking(false);
		clientChannel.register(key.selector(), SelectionKey.OP_READ, ByteBuffer.allocate(bufferSize));
	}

	public void handleRead(SelectionKey key) throws IOException {
		// 获得与客户端通信的信道
		SocketChannel clientChannel = (SocketChannel) key.channel();
		// 得到并清空缓冲区
		ByteBuffer buffer = (ByteBuffer) key.attachment();
		buffer.clear();

		// 读取信息获得读取的字节数
		long bytesRead = clientChannel.read(buffer);

		if (bytesRead == -1) {
			// 没有读取到内容的情况
			clientChannel.close();
		} else {
			// 将缓冲区准备为数据传出状态
			buffer.flip();

			// 将字节转化为为UTF-16的字符串
			String receivedString = Charset.forName("UTF-8").newDecoder().decode(buffer).toString();

			byte[] b= clientChannel.socket().getInetAddress().getAddress();
//			for(byte bb : b){
//				System.out.println("bb:"+bb);
//			}
			System.out.println(clientChannel.socket().getInetAddress().getHostAddress());
			
			// 控制台打印出来
			System.out.println("接收到来自" + clientChannel.socket().getInetAddress().getHostAddress() + "的信息:" + receivedString);

			//命令交由处理装置分解并分派到后台运行
			String result = "";
			try {
				result = command.accept(receivedString,  clientChannel.socket().getInetAddress().getHostAddress(),null);
			} catch (UserTimeoutException e) {
				result = "您已超时，请重新登录!";
				e.printStackTrace();
			}
			
			
			// 准备发送的文本
//			String sendString = "你好,客户端. @" + new Date().toString() + "，已经收到你的信息" ;
			//将处理结果发送给客户端
			buffer = ByteBuffer.wrap(result.getBytes("UTF-8"));
			handleWrite(clientChannel, buffer);

			// 设置为下一次读取或是写入做准备
			key.interestOps(SelectionKey.OP_READ | SelectionKey.OP_WRITE);
		}
	}

	public void handleWrite(SocketChannel clientChannel, ByteBuffer buffer) throws IOException {
		clientChannel.write(buffer);
	}
	
	public void handleAccept(SelectionKey key) throws IOException {
//		SocketChannel clientChannel = (SocketChannel) key.channel();
//		Socket socket = clientChannel.socket();
//		System.out.println(socket.getLocalSocketAddress());
//		System.out.println(socket.getRemoteSocketAddress());
	}

	public int getBufferSize() {
		return bufferSize;
	}

	public void setBufferSize(int bufferSize) {
		this.bufferSize = bufferSize;
	}
}