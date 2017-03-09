package com.hola.common.exception;

public class BaseException extends Exception
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -2279557533332869636L;

	public BaseException(String message) {
		super(message);
	}

	public BaseException(Throwable cause) {
		super(cause);
	}

	public BaseException(String message, Throwable cause) {
		super(message, cause);
	}
}
