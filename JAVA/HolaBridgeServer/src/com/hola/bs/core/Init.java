package com.hola.bs.core;

import java.util.Map;
import java.util.ResourceBundle;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;

import org.springframework.context.ApplicationContext;

import com.hola.bs.bean.CommandContainer;

public class Init {
	
	/**
	 * 增加一个Map，存放接收指令信息用,初始空间为10
	 */
	public static Map<String,CommandContainer> commContainerMap = new ConcurrentHashMap<String,CommandContainer>(10);
	
	/**
	 * 增加一个MAP，存放打印IP地址
	 */
	public static Map<String,String> printIpMap;
	
	public static ApplicationContext ctx;
	public static long startMills;
	
//	public static int test_id = 0;
	
	public static Map<String,String> loadPrintInfoIntoMap(){
		printIpMap = new ConcurrentHashMap<String, String>();
		ResourceBundle source = ResourceBundle.getBundle("server_ip_address");
		Set<String> keys = source.keySet();
		for(String s : keys){
			//System.out.println("key = " + s +", value = "+ source.getString(s));
			printIpMap.put(s, source.getString(s));
		}
		
		return printIpMap;
	}
	

}
