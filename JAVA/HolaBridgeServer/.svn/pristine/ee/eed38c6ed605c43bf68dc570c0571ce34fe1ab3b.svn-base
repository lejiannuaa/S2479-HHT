package com.hola.bs.impl;

import java.io.IOException;
import java.sql.SQLException;

import com.hola.bs.util.Config;
import com.hola.bs.util.Root;
import com.hola.bs.util.XmlElement;

/**
 * PO收货查询 <p>
 * 请求消息：request=004;usr=XXXX;op=N;bc=YYYY;from=TTTT;to=TTTT;state=N;opusr=N <p>
 * 响应消息：response=004;code=N;desc=XXXX <p>
 * 写入xml:
 * <root>
 *  <config>
 *      <type/>
 *      <direction/>
 *      <id/>
 *  </config>
 *  <info>
 *      <PO单号/>
 *      <厂商编号/>
 *      <厂商名称/>
 *      <收货状态/>
 *      <预计到货日/>
 *  </info>
 * </root>
 * @author roy
 *
 */
public class HHT_201_004 extends AbstractProcessUnit {
    // 收货：0
    private String type = "0";
    // Server->Client：0
    private String direction = "0";
    // 接口编号：1位接口类型+2位接口序号+2位操作序号
    private String id;
    
    public String process(Request req) {
    	// 请求类型[查询=01,申请收货=02,解除收货=03,收货=04]
    	String op = req.getParameter("op");
    	// 单号
    	String bc = req.getParameter("bc");
    	// 起始日期
    	String from = req.getParameter("from");
    	// 截止日期
    	String to = req.getParameter("to");
    	// 单状态
    	String state = req.getParameter("state");
    	// 操作人
    	String opusr = req.getParameter("opusr");

    	Root root = new Root();
    	root.addConfig(new Config(type, direction, id));
    	root.addXmlElement(new XmlElement("info", queryBySqlId("info")));

    	try {
    		System.out.println(root.asXml());
    	} catch (SQLException e) {
    		e.printStackTrace();
    	} catch (IOException e) {
    		e.printStackTrace();
    	}

    	return null;
    }
    @Override
    public String getProcessId() {
        return this.getClass().getSimpleName();
    }

    
}
