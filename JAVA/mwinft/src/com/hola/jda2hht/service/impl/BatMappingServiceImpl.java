package com.hola.jda2hht.service.impl;

import java.util.Date;

import com.hola.jda2hht.dao.BaseDao;
import com.hola.jda2hht.model.BatMappingBean;
import com.hola.jda2hht.service.IBatMappingService;

public class BatMappingServiceImpl implements IBatMappingService {
	private BaseDao dao;

	public BaseDao getDao() {
		return dao;
	}

	public void setDao(BaseDao dao) {
		this.dao = dao;
	}

	@Override
	public void addMapping(BatMappingBean bean) {
		String sql = "insert into HOLA_APP_BATMAPPING (CHGCODE,INSTNO,FILNAME,CRTTIME)values(?,?,?,?)";
		this.dao.update(sql, new Object[] { bean.getChgcode(),
				bean.getInstno(), bean.getFilname(), new Date() });
	}

}
