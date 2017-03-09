package com.hola.bs.core.exception;

/**
 * 用户登录已超时
 * @author S1608
 *
 */
public class UserTimeoutException extends Exception {
	public UserTimeoutException(String msg){
		super(msg);
	}
}
