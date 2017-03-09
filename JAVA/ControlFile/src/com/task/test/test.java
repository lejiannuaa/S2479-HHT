package com.task.test;

import java.lang.reflect.Constructor;
import java.lang.reflect.Method;
import java.util.Hashtable;
import java.util.Iterator;

public class test {

	public void test1(String s){
		System.out.println("===test1==="+s);
	}
	
	public void test1(){
		System.out.println("===test1===");
	}
	
	public void test2(){
		System.out.println("===test2===");
	}
	
	public Hashtable test3(String str1,String str2){
		System.out.println("===str1==="+str1);
		System.out.println("===str2==="+str2);
		Hashtable ht = new Hashtable();
		ht.put("str1", "1");
		ht.put("str2", "2");
		return ht;
	}
	
	public static void main(String[] args){
		try{
			Class cl = Class.forName("com.task.test.test");
			Class[] strArgsClass = new Class[] {};
			Constructor constructor = cl.getConstructor(strArgsClass);
			Object[] strArgs = new Object[] {};
	        Object object = constructor.newInstance(strArgs);
	        
	        Class[] c = new Class[2];
	        c[0] = String.class;
	        c[1] = String.class;
	        Method meth = cl.getMethod("test3", c);
	        Hashtable ht = (Hashtable) meth.invoke(object, "aa","bb");
	        Iterator it = ht.keySet().iterator();
	        while(it.hasNext()){
	        	String key = it.next().toString();
	        	System.out.println("key="+key+"  val="+ht.get(key));
	        }
		}catch(Exception e){
			e.printStackTrace();
		}
	}
}
