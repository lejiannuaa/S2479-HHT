package com.hola.bs.core.exception;

/**
 * 指令对应的处理单元不存在
 * @author S1608
 *
 */
public class ProcessUnitNotException extends Exception {
	public ProcessUnitNotException(String msg){
		super(msg);
	}
}
