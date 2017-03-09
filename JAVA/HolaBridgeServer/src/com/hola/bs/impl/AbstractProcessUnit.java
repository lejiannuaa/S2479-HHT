package com.hola.bs.impl;

import java.util.List;
import java.util.Map;

import com.hola.bs.util.SpringUtil;

/**
 * 处理单元抽象类。
 * <p>
 * 把字符串请求转换为Request对象。获取共用的request和usr参数。<br>
 * 响应消息：response=004;code=N;desc=XXXX<br>
 * 
 * @author S1788
 * 
 */
public abstract class AbstractProcessUnit implements ProcessUnit {
    private Request req = null;
    private Response response = new Response();
    private String request;
    private String usr;

    public String process(String requestStr) {
        req = new Request(requestStr);
        this.request = req.getParameter(Request.REQUEST);
        this.usr = req.getParameter(Request.USR);
        response.setResponse(this.request);
        try {
            process(req);
        } catch (Exception e) {
            e.printStackTrace();
            
            response.setCode(Response.FAIL);
            response.setDesc(e.getMessage());
            return response.toString();
        }

        return response.toString();
    }

    public abstract String getProcessId();

    protected List<Map> queryBySqlId(String id) {
        return SpringUtil.searchForList(SqlConfig.get(getProcessId() + "_" + id));
    }

    public String getRequest() {
        return request;
    }

    public String getUsr() {
        return usr;
    }

}
