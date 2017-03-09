package com.hola.common;

import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

public class SpringContextHelper 
{
	private		static		SpringContextHelper		springContextHelper;
	private		final	ApplicationContext		applicationContext	=	
						new ClassPathXmlApplicationContext("applicationContext.xml");
	
	private	SpringContextHelper()
	{}
	public static SpringContextHelper getInstance()
	{
		if(springContextHelper == null)
			springContextHelper = new SpringContextHelper();
		return springContextHelper;
	}
	
	public Object getBean(String beanName)
	{
		return this.applicationContext.getBean(beanName);
	}
}
