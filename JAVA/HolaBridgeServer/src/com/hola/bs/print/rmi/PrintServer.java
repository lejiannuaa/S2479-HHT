package com.hola.bs.print.rmi;

public interface PrintServer {

	public void print(String fileName);
	
	public void testRemotePrint()throws Exception;
}
