package com.hola.common.exception;

public class FileNameRepeat extends BaseException 
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	public FileNameRepeat(String message) 
	{
		super("生成文件名相同！详情：" + message);
	}
}
