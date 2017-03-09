package com.hola.bs.service.hht;

import java.util.ArrayList;
import java.util.List;

import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.print.template.PrintTemplate;

/**
 * 模板7打印类
 * @author S1788
 *
 */
public class HHT_999_07 implements ProcessUnit {

    public String process(Request request) {
        // TODO 
        // 解析request为sql params
        
        List<String> paramsList = new ArrayList<String>();
        paramsList.add("1");
        String[] params = new String[paramsList.size()];
        paramsList.toArray(params);
        return PrintTemplate.printByTemplateNo("7",params);
    }

    public static void main(String[] args) {
//        ApplicationContext ctx=new ClassPathXmlApplicationContext("spring.xml");
//        new HHT_999_07().process("1");
    }
}
