package com.hola.bs.service.hht.strategy_HHT_203_02;

import java.util.Map;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.print.rmi.PrintServer;

/**
 * 策略模式的上下文环境
 * @author S2139
 * 2012 Jun 13, 2012 5:22:21 PM 
 */
public class Context {
	
	private AbstractProcess_HHT_203_02 abstract_203_02;
	
	public Context(){
		
	}

	public Context(AbstractProcess_HHT_203_02 abstract_203_02,PrintServer server){
		this.abstract_203_02 = abstract_203_02;
		this.abstract_203_02.setPrintServer(server);
	}
	
	public AbstractProcess_HHT_203_02 getAbstract_203_02() {
		return abstract_203_02;
	}

	public void setAbstract_203_02(AbstractProcess_HHT_203_02 abstract_203_02) {
		this.abstract_203_02 = abstract_203_02;
	}
	
	public void executePrint(Map[] data, BusinessBean bean)throws Exception{
		this.getAbstract_203_02().executePrint(data, bean);
	}
	
}
