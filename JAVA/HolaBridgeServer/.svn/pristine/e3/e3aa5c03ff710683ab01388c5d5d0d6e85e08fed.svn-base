package com.hola.bs.core.interceptor;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.aop.ThrowsAdvice;
import org.springframework.remoting.RemoteLookupFailureException;

import com.hola.bs.core.exception.Template6Exception;

/**
 * AOP方式捕获异常，防止在控制台上打印出异常信息
 * @author S2139
 * 2012 Jun 12, 2012 4:24:31 PM 
 */
public class ExceptionHandler implements ThrowsAdvice {
	
	Log log = LogFactory.getLog(getClass());
	
	public void afterThrowing(RemoteLookupFailureException rlfe){
		log.error("调用远程服务出现异常："+rlfe.getMessage());
	}
	
	public void afterThrowing(Template6Exception t6e){
		log.error("未取得调入店信息。", t6e);
	}
	
//	public void afterThrowing(Exception e){
////		System.out.println("抛出异常");
//		log.error(e);
//	}
}
