package com.hola.jda2hht.service.impl;

import org.springframework.jdbc.core.JdbcTemplate;

import com.hola.jda2hht.dao.BaseDao;
import com.hola.jda2hht.model.ChangeSysBean;
import com.hola.jda2hht.util.ConfigUtil;

public class BaseJDAService {

	protected BaseDao dao = new BaseDao();
	protected String schma;

	public BaseJDAService(ChangeSysBean sysBean) throws Exception {
		// 这里必须在程序中动态产生连接，动态产生templet对象
		JdbcTemplate templet = ConfigUtil.createTemlet(sysBean);
		dao.setJdbcTemplate(templet);
		// 获得表空间
		this.schma = sysBean.getDbschema();
	}
}
