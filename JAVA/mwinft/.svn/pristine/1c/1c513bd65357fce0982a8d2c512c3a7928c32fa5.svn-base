package com.hola.jda2hht.service.impl;

import java.util.List;

import com.hola.jda2hht.model.ChangeSysBean;
import com.hola.jda2hht.model.JDAChangeInstfBean;
import com.hola.jda2hht.service.IJDAChgInstfService;

public class JDAChgInstfServiceImpl extends BaseJDAService implements
		IJDAChgInstfService {

	public JDAChgInstfServiceImpl(ChangeSysBean sysBean) throws Exception {
		super(sysBean);
	}

	@Override
	@SuppressWarnings("unchecked")
	public List<JDAChangeInstfBean> getAllIntList(String instno) {
		String sql = "select INSTNO,OTHSUM,SRCCNT,SRCLIB,SRCNAM,"
				+ "SRCSUM,TARCNT,TARLIB,TARNAM,TAROTH,TARSUM,TMPLIB,"
				+ "TMPNAM from " + schma + ".CHGINSTF where INSTNO=?";
		return (List<JDAChangeInstfBean>) this.dao.getList(sql,
				new Object[] { instno }, JDAChangeInstfBean.class);
	}

}
