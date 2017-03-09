package com.hola.bs.core.exception;

/**
 * 用户非本门店员工异常
 * @author S1608
 *
 */
public class UserNotSameStoreException extends Exception {
	public UserNotSameStoreException(String msg){
		super(msg);
	}
}
