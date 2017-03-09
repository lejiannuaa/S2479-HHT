package com.hola.bs.socket;

import java.io.IOException;
import java.nio.BufferUnderflowException;
import java.nio.channels.ClosedChannelException;
import java.util.Date;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.xsocket.MaxReadSizeExceededException;
import org.xsocket.connection.IConnectHandler;
import org.xsocket.connection.IConnectionTimeoutHandler;
import org.xsocket.connection.IDataHandler;
import org.xsocket.connection.IDisconnectHandler;
import org.xsocket.connection.IIdleTimeoutHandler;
import org.xsocket.connection.INonBlockingConnection;

import com.hola.bs.core.Command;
import com.hola.bs.core.Init;
import com.hola.bs.pool.ThreadFactoryMBImpl;
import com.hola.bs.util.DateUtils;

public class XServerHandler implements IDataHandler, IConnectHandler, IIdleTimeoutHandler, IConnectionTimeoutHandler,
		IDisconnectHandler {
	public Log log = LogFactory.getLog(XServerHandler.class);
	/**
	 * 命令处理装置
	 */
	@Autowired(required = true)
	private Command command;

	public Log log2 = LogFactory.getLog("sysReceiveLog");
	
	public Command getCommand() {
		return command;
	}

	public void setCommand(Command command) {
		this.command = command;
	}

	/**
	 * 即当建立完连接之后可以进行的一些相关操作处理。包括修改连接属性、准备资源、等！ 连接的成功时的操作
	 */
//	public void messageReceived(IoSession session, Object message) throws Exception {
//		String msg = message.toString();
//		System.out.println("服务端接收到的数据为：" + session.getRemoteAddress().toString());
//		if ("bye".equals(msg)) {
//			// 服务端断开连接的条件
//			session.close(true);
//		}
////		Msg m = new Msg();
////		Date date = new Date();
//		System.out.println("========="+msg);
//		String request = command.accept(msg, session.getLocalAddress().toString());
	
	public boolean onConnect(INonBlockingConnection nbc) throws IOException, BufferUnderflowException,
			MaxReadSizeExceededException {
		if(nbc == null){
			log2.info("未知的或空连接的对象请求接入bridgeServer,请联系管理员协助检查。");
		}else{
			nbc.getRemoteAddress().getHostAddress();
			nbc.setMaxReadBufferThreshold(16384);
			log2.info("收到新的连接，connectionId = "+nbc.getId()+ ", time: "+DateUtils.string2TotalTime(new Date()));
		}
		
//		System.out.println("="+nbc.getRemoteAddress().getHostAddress());
//		System.out.println("="+nbc.getRemoteAddress().getHostName());
//		System.out.println("="+nbc.getRemoteAddress().getAddress());
//		System.out.println("="+nbc.getRemoteAddress().getCanonicalHostName());
//		System.out.println("="+nbc.getRemoteAddress().get);
		
//		byte[] remoteName = nbc.getRemoteAddress().getAddress();
//		for(byte b : remoteName){
//			System.out.println("="+b);
//		}
//		System.out.println("Server : remoteName " + remoteName + " has connected ！");
		return true;
	}

	/**
	 * 即如果失去连接应当如何处理？ 需要实现 IDisconnectHandler 这个接口 连接断开时的操作
	 */
	public boolean onDisconnect(INonBlockingConnection nbc) throws IOException {
//		System.out.println("now close...");
		log2.info("关闭连接! "+ "connectionId = "+nbc.getId() + ", time: "+DateUtils.string2TotalTime(new Date()));
		if(Init.commContainerMap.containsKey(nbc.getId())){
			Init.commContainerMap.remove(nbc.getId());
		}
		return false;
	}

	/**
	 * 即这个方法不光是说当接收到一个新的网络包的时候会调用而且如果有新的缓存存在的时候也会被调用。 而且当连接被关闭的时候也会被调用的! The
	 * onData will also be called, if the connection is closed
	 */
	public boolean onData(INonBlockingConnection nbc) throws IOException, BufferUnderflowException,
			ClosedChannelException, MaxReadSizeExceededException {

//		byte[] data = nbc.readBytesByLength(nbc.available());// 接受字节数据
//		ByteBuffer b = ByteBuffer.wrap(data);// 组装成字节缓存
//		String receivedString = Charset.forName("UTF-8").newDecoder().decode(b).toString();// 解析缓存成为字符串
//
//		log.info("接收到来自" + nbc.getRemoteAddress().getHostAddress() + "的信息:" + receivedString);//输出到日志
//		//System.out.println("接收到来自" + nbc.getRemoteAddress().getHostAddress() + "的信息:" + receivedString);
//
//		// 命令交由处理装置分解并分派到后台运行
//		String result = "";
//		try {
//			result = command.accept(receivedString, nbc.getRemoteAddress().getHostAddress());
//		} catch (UserTimeoutException e) {
//			result = "您已超时，请重新登录!";
//			e.printStackTrace();
//		}
//		System.out.println("输出到：" + result);
//		nbc.write(result);// 写出到客户端
//		System.out.println("..."+nbc.isOpen());
//		System.out.println("XBridgeServer接收到请求的开始时间："+ DateUtils.DateFormatToString(DateUtils.getSystemDate(),"yyyy-MM-dd HH:mm:ss.SSS") + "    "+ System.nanoTime());
//		log.info("XBridgeServer接收到请求的开始时间："+ DateUtils.DateFormatToString(DateUtils.getSystemDate(),"yyyy-MM-dd HH:mm:ss.SSS") + "    "+ System.nanoTime());
		log.info("接收到数据 ! "+ "connectionId = "+nbc.getId() + ", time: "+DateUtils.string2TotalTime(new Date()));
//--------------------------------------------------------------------------------------------------------------	
		new ThreadFactoryMBImpl().runJob(command, nbc);
		try {
			Thread.sleep(10);
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

//--------------------------------------------------------------------------------------------------------------
		return true;
	}

	/**
	 * 请求处理超时的处理事件
	 */
	public boolean onIdleTimeout(INonBlockingConnection nbc) throws IOException {
		if(Init.commContainerMap.containsKey(nbc.getId())){
			Init.commContainerMap.remove(nbc.getId());
		}
		log2.info("IdleTimeout! "+ "connectionId = "+nbc.getId() + ", time: "+DateUtils.string2TotalTime(new Date()));
		return false;
	}

	/**
	 * 连接超时处理事件
	 */
	public boolean onConnectionTimeout(INonBlockingConnection nbc) throws IOException {
		if(Init.commContainerMap.containsKey(nbc.getId())){
			Init.commContainerMap.remove(nbc.getId());
		}
		log2.info("Connect timeout! "+ "connectionId = "+nbc.getId() + ", time: "+DateUtils.string2TotalTime(new Date()));
		return false;
	}

	
}
