package com.hola.bs.print.rmi;

import org.springframework.remoting.rmi.RmiProxyFactoryBean;

/**
 * 动态RMI客户端
 * @author S2139
 * 2012 Sep 25, 2012 10:03:52 AM 
 */
public class DynamicRmiClient {
	
	/**
	 * 打印操作的服务名
	 */
	public static final String REMOTE_PRINT_NAME = "print";
	
	/**
	 * 打印操作服务暴露的接口（不建议做成配置，改成强制约定）
	 */
	public static final String REMOTE_PRINT_PORT="8091";
	
	/**
	 * @param ip 打印服务的IP地址
	 * @param serviceName 打印服务名称 传静态常量
	 * @param port 打印服务端口
	 * @return 打印服务客户端接口对象
	 * author: S2139
	 * 2012 Sep 25, 2012 10:04:58 AM
	 */
	public PrintServer getRmiPrintClient(String ip,String serviceName,String port){
		RmiProxyFactoryBean factory = new RmiProxyFactoryBean();
		factory.setServiceInterface(PrintServer.class);
		factory.setServiceUrl(generateRmiAddress(ip,serviceName,port));
		factory.setRefreshStubOnConnectFailure(true);
		factory.setLookupStubOnStartup(false);
		RMICustomClientSocketFactory rccs = new RMICustomClientSocketFactory();
		rccs.setTimeout(5000);
		factory.setRegistryClientSocketFactory(rccs);
		factory.afterPropertiesSet();
		PrintServer pServer = (PrintServer) factory.getObject();
		return pServer;
	}
	
	/**
	 * 生成RMI访问地址，参数不再阐述
	 * @param ip
	 * @param serviceName
	 * @param port
	 * @return
	 * author: S2139
	 * 2012 Sep 25, 2012 10:08:44 AM
	 */
	private String generateRmiAddress(String ip, String serviceName,String port){
		StringBuffer sb = new StringBuffer();
		sb.append("rmi://").append(ip).append(":").append(port).append("/").append(serviceName);
		return sb.toString();
	}
	
	public static void main(String[] args){
		System.out.println(new DynamicRmiClient().generateRmiAddress("192.168.0.100", "print", "8091"));
	}
}
