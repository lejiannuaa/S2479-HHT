package com.hola.bs.socket;

import java.io.IOException;
import java.net.UnknownHostException;
import java.util.List;
import java.util.concurrent.TimeUnit;

import org.apache.log4j.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;
import org.xsocket.WorkerPool;
import org.xsocket.connection.IServer;
import org.xsocket.connection.Server;
import org.xsocket.connection.IConnection.FlushMode;

import com.hola.bs.core.Init;
import com.hola.bs.pool.ThreadFactoryMBImpl;
import com.hola.bs.property.ConfigPropertyUtil;
import com.hola.bs.util.SpringUtil;

public class XBridgeServer {


	private static Logger logger = Logger.getLogger(XBridgeServer.class);

	@Autowired(required = true)
	private XServerHandler handler = null;


	/** 设置当前的端口 */
//	private static final int PORT = 1978;
	public void start() {
		ConfigPropertyUtil c = new ConfigPropertyUtil();
		
//		String sql = "SELECT count(1) FROM hhtserver.hhtinit";
//		List list = SpringUtil.searchForList(sql);

		// 创建一个服务端的对象
		IServer srv = null;
		try {
			srv = new Server(c.getValue("serverIp"), c.getIntValue("port"), handler);
			
			// 设置当前的采用的异步模式
			srv.setFlushmode(FlushMode.ASYNC);
			try {
				//minSize, maxSize, keepalive, timeunit, taskqueuesize, isDaemon
				WorkerPool workerPool = new WorkerPool(50, 300, 5000, TimeUnit.SECONDS, 1000, false);
				workerPool.prestartAllCoreThreads();// 预启动所有核心线程
				srv.setWorkerpool(workerPool);
				
				srv.setIdleTimeoutMillis(60000);//超时一分钟
				srv.setConnectionTimeoutMillis(60000);//连接中断超过一分钟
				
				srv.start(); // returns after the server has been started
				
				new ThreadFactoryMBImpl().initialize();
				
//				System.out.println("服务器" + srv.getLocalAddress() + ":" + PORT);
//				Map<String, Class> maps = srv.getOptions();
//				if (maps != null) {
//					for (Entry<String, Class> entry : maps.entrySet()) {
//						System.out.println("key= " + entry.getKey() + " value =" + entry.getValue().getName());
//					}
//				}
//				System.out.println("info: " + srv.getStartUpLogMessage()+"  "+System.currentTimeMillis());

			} catch (Exception e) {
				System.out.println(e);
			}

		} catch (UnknownHostException e1) {
			e1.printStackTrace();
		} catch (IOException e1) {
			e1.printStackTrace();
		}
	}

	public XServerHandler getHandler() {
		return handler;
	}

	public void setHandler(XServerHandler handler) {
		this.handler = handler;
	}

	public static void main(String[] args) {
		System.out.println("正在初始化容器……");
		ApplicationContext ctx = new ClassPathXmlApplicationContext("spring.xml");
		System.out.println("容器初始化成功！");
		Init.ctx = ctx;
//		System.out.println("加载MQ定时器……");
//		new SendMqTimer();
//		System.out.println("MQ定时器加载成功！");
		System.out.println("加载打印服务配置信息！");
		Init.loadPrintInfoIntoMap();
		System.out.println("加载打印服务配置信息成功！");
		XBridgeServer serve = (XBridgeServer) ctx.getBean("Xserver");
		serve.start();
		System.out.println("version:V1.51212"+"  HHT开始监听客户信息请求…");
	}
	
	
	

}
