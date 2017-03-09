package com.hola.bs.socket;

import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.channels.SelectionKey;
import java.nio.channels.SocketChannel;

/**
 * TCPServerSelector与特定协议间通信的接口
 * 
 * @date 2010-2-3
 * @time 上午08:42:42
 * @version 1.00
 */
public interface TCPProtocol {

	/**
	 * SocketChannel的请求处理完成
	 * 
	 * @throws IOException
	 */
	void handleAccepted(SelectionKey key) throws IOException;

	/**
	 * 
	 * 接收到一个SocketChannel的请求，准备进行处理
	 * （目前只有一些日志记录的工作而已）
	 * @param key
	 * @throws IOException
	 */
	void handleAccept(SelectionKey key) throws IOException;

	/**
	 * 从一个SocketChannel读取信息的处理
	 * 
	 * @param key
	 * @throws IOException
	 */
	void handleRead(SelectionKey key) throws IOException;

	/**
	 * 向一个SocketChannel写入信息的处理
	 * 
	 * @param clientChannel 客户端通道
	 * @param buffer 需要输出的数据
	 * @throws IOException
	 */
	void handleWrite(SocketChannel clientChannel, ByteBuffer buffer) throws IOException;
}