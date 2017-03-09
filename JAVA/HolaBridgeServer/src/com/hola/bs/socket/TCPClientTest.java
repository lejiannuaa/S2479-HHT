package com.hola.bs.socket;

import java.io.IOException;

public class TCPClientTest extends Thread{

	private String threadname;
	public TCPClientTest(){
		super();
	}
	
	public TCPClientTest(String threadname){
		this.threadname = threadname;
	}
	
	public void run(){
		TCPClient t;
		try {
			t = new TCPClient("10.130.254.18", 1978);
			t.sendMsg("threadn");
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	
	public String getThreadname() {
		return threadname;
	}

	public void setThreadname(String threadname) {
		this.threadname = threadname;
	}

	public static void main(String[] args){
		try {
//			TCPClient t = new TCPClient("192.168.1.100", 1978);
//			new ThreadFactoryMBImpl().initialize();
//			for(int i=0; i<1; i++){
////				System.out.println("i==="+i);
//				new ThreadFactoryMBImpl().runJob(Integer.toString(i));
////				t.sendMsg("threadname="+threadname+";request=test;usr=T2215;op=01;from=20120101;to=20120430;bcfrom=000001;bcto=99999;boxbcfrom=;boxbcto=;type=TRF;state=Y;frml=HHT;count="+i+";\n");
//			}
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		for(int i=0; i<1; i++){
			Thread t1 = new TCPClientTest("t1");
			t1.start();
		}
		
//		Thread t2 = new TCPClientTest("t2");
//		Thread t3 = new TCPClientTest("t3");
//		Thread t4 = new TCPClientTest("t4");
//		Thread t5 = new TCPClientTest("t5");
//		Thread t6 = new TCPClientTest("t6");
//		
		
//		t2.start();
//		t3.start();
//		t4.start();
//		t5.start();
//		t6.start();
	}
}
