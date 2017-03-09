package com.hola.bs.core;

import java.math.BigDecimal;
import java.util.Iterator;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import com.hola.bs.bean.UseVerify;
import com.hola.bs.bean.UserState;
import com.hola.bs.core.exception.UserAlreadyRegisterException;
import com.hola.bs.core.exception.UserNotExistException;
import com.hola.bs.core.exception.UserNotSameStoreException;
import com.hola.bs.core.exception.UserPasswordException;
import com.hola.bs.core.exception.UserTimeoutException;
import com.hola.bs.impl.Request;

/**
 * 用户管理容器。
 * 所有登录用户必须在此注册。
 * 该对象为单例对象。
 * @author S1608
 *
 */
public class UserContainer {
	
	public Log log = LogFactory.getLog("sysMonitorLogger");
	
	/**
	 * 核心容器
	 */
	private ConcurrentHashMap<String, UserState> userMap = null;
	
	//用户验证对象
	private UseVerify uv = null;
	
	private UserContainer(){
		userMap = new ConcurrentHashMap<String, UserState>();
	}
	
	/**
	 * 用户注册
	 * @param ip 用户所在IP
	 * @param name 用户id是由门店号加用户名组成的字符串
	 * @param password 用户密码
	 * 
	 * @throws UserAlreadyRegisterException 用户已经登录异常
	 * @throws UserTimeoutException  用户登录已经超时异常
	 * @throws UserNotExistException 用户不存在数据库异常
	 * @throws UserPasswordException 用户密码错误异常
	 * @throws UserNotSameStoreException 用户非本门店员工异常
	 */
	public void register(Request request){
		
			//登录成功，注册用户
			UserState user = new UserState();
			
			user.setIp(request.getParameter("userip"));

			user.setName(request.getParameter(Request.USR));

			//建议将store ip的关系放入缓存中
			user.setStore(uv.getStoreByIP(request.getParameter("userip")));

			
			user.setOwnerFilePath(request.getParameter(Request.USR));

//			user.setLoginTime(System.currentTimeMillis());
			
//			user.setAttribute("fromlocation", uv.getFromLocation());
//			
//			user.setAttribute("tolocation", uv.getToLocation());

			synchronized(userMap){
				//如果密码正确，允许登录
				userMap.put(request.getParameter(Request.USR), user);
			}
//			Set<String> set = userMap.keySet();
//			Iterator iter = set.iterator();
//			System.out.println("****************日志信息展示*********************");
//			while(iter.hasNext()){
//				String name = String.valueOf(iter.next());
//				UserState uus = userMap.get(name);
//				System.out.println(name + "  "+uus.getIp()+"  "+uus.getStore());
//			}
//			System.out.println("****************日志信息结束*********************");
			sysLog();
	}
	
	/**
	 * 获得已登录用户的信息
	 * @param user
	 * @return
	 */
	public synchronized UserState getUser(String user){
		return userMap.get(user);
	}
	
	/**
	 * 注销用户
	 * @param name
	 */
	public void logout(String name){
		userMap.remove(name);
		sysLog();
	}

	public UseVerify getUv() {
		return uv;
	}

	public void setUv(UseVerify uv) {
		this.uv = uv;
	}

	/**
	 * 增加记录系统日志的操作
	 * @param userName
	 * @param action 行为名称
	 * author: S2139
	 * 2012 Sep 7, 2012 4:06:36 PM
	 */
	private void sysLog(){
//		Runtime rt = Runtime.getRuntime();
//		MDC.put("userNo", userName);
//		MDC.put("onlineCount", userMap.size());
//		MDC.put("freeMemory", convertIntoMB(rt.freeMemory()));
//		MDC.put("maxMemoryAvailable", convertIntoMB(rt.maxMemory()));
		Set<String> set = userMap.keySet();
		if(set.size()>0){
			Iterator iter = set.iterator();
			log.info("========================当前在线用户信息统计：===========================");
			while(iter.hasNext()){
				String name = String.valueOf(iter.next());
				UserState uus = userMap.get(name);
				log.info("用户名："+name + "  "+"用户IP："+uus.getIp()+"  "+"所在店铺："+uus.getStore());
			}
			log.info("=====================================================================");
		}
	}
	
	/**
	 * 单独的一个方法，将内存的字节数转换成MB
	 * @param bytes
	 * author: S2139
	 * 2012 Sep 7, 2012 4:15:44 PM
	 */
	private String convertIntoMB(long bytes){
		BigDecimal b = new BigDecimal(bytes);
		BigDecimal b1024 = new BigDecimal(1024);
		b = b.divide(b1024).divide(b1024,0,BigDecimal.ROUND_HALF_DOWN);
		return b.toString()+"MB";
	}


	
}
