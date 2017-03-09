package com.hola.bs.socket;


public class BridgeServer {

//	private static Logger logger = Logger.getLogger(BridgeServer.class);
//	private static int PORT = 1978;
//
//	public static void main(String[] args) {
//		
//		ApplicationContext ctx=new ClassPathXmlApplicationContext("spring.xml");
//		BridgeServer serve = (BridgeServer)ctx.getBean("minaServer");
//		serve.start();
//			
//	}
//
//	public void start() {
//
//		IoAcceptor acceptor = null;
//		try {
//			// 创建一个非阻塞的server端的Socket;
//			acceptor = new NioSocketAcceptor();
//			acceptor.getFilterChain().addLast(
//					"codec",
//					new ProtocolCodecFilter(new TextLineCodecFactory(Charset.forName("UTF-8"), LineDelimiter.WINDOWS
//							.getValue(), LineDelimiter.WINDOWS.getValue())));
//			// 设置读取数据的缓冲区大小
//			acceptor.getSessionConfig().setReadBufferSize(1024 * 5);
//			// 读写通道10秒内无操作进入空闲状态
//			acceptor.getSessionConfig().setIdleTime(IdleStatus.BOTH_IDLE, 10);
//			// 绑定逻辑处理器
//			acceptor.setHandler(new ServerHandler());
//			// 绑定端口
//			acceptor.bind(new InetSocketAddress(PORT));
//			logger.info("服务端启动成功... 端口号：" + PORT);
//		} catch (Exception e) {
//			e.printStackTrace();
//		}
//
//	}

}
