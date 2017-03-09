package com.hola.bs.impl;

/**
 * 所有负责处理BridgeServer命令的程序都必须实现该接口<br>
 * 该接口将会返回处理结果，如果成功，则返回"true"，如果失败，则返回"false:异常原因"<br>
 * @author S1608
 *
 */
public interface ProcessUnit {
	
	/**
	 * 处理方法
	 * @param request 指令
	 * @return 处理结果字符串："true"/"false:XXX异常发生"
	 */
//	public String process(String request);
	
	public String process(Request request);

}
