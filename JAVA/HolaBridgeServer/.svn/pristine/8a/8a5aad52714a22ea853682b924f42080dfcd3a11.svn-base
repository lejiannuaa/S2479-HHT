package com.hola.bs.util;

import java.util.List;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.support.rowset.SqlRowSet;

import com.hola.bs.core.Init;

public class SpringUtil {
    
    private static ApplicationContext context = Init.ctx;
    private static Log log = LogFactory.getLog(SpringUtil.class);
    
    public static ApplicationContext getContext(){
        if ( context == null ){
            context = new ClassPathXmlApplicationContext("spring.xml");
        }
        return context;
    }
    
    /**
     * 手动获取BEAN对象
     * @param beanName
     * @return
     * author: S2139
     * 2012 Nov 8, 2012 10:00:19 AM
     */
    public static Object getBean(String beanName){
    	
    	return getContext().getBean(beanName);
    }
    
    public static JdbcTemplate getJdbctemplate(){
    	
        return getContext().getBean(JdbcTemplate.class);
    }
    
    public static List searchForList(String sql){
    	if(sql!=null){
			log.info("执行语句="+sql);
		}
        return getJdbctemplate().queryForList(sql);
    }
    
    public static List searchForList(String sql, Object[] ages){
		if(sql!=null){
			log.info("执行语句="+sql);
		}
		if(ages!=null){
			for(Object o : ages){
				if(o instanceof Object[]){
					Object[] oo = (Object[]) o;
					String s = "";
					for(Object o1 : oo){
						s += o1+"\t";
					}
					log.info("参数="+s);
				}else{
					log.info("参数="+o);
				}
			}
		}
    	return getJdbctemplate().queryForList(sql, ages);
    }
    
    public static int update(String sql){
    	if(sql!=null){
			log.info("执行语句="+sql);
		}
    	return getJdbctemplate().update(sql);
    }
    
    public static SqlRowSet searchForTest(String sql){
    	if(sql!=null){
			log.info("执行语句="+sql);
		}
        return getJdbctemplate().queryForRowSet(sql);
    }
 
}
