package com.hola.jda2hht.service.impl;

import com.hola.jda2hht.dao.BaseDao;
import com.hola.jda2hht.model.ChangeTableBean;
import com.hola.jda2hht.service.IChgTblService;

public class ChgTblServiceImpl implements IChgTblService {
	private BaseDao dao;

	public BaseDao getDao() {
		return dao;
	}

	public void setDao(BaseDao dao) {
		this.dao = dao;
	}

	@Override
	public ChangeTableBean getChageTable(String changeCode, String tableName) {
		String sql = "select ID,CHGCODE,SRCTBLNAME,TARTBLNAME from HOLA_APP_CFG_CHGTBL where CHGCODE=? and SRCTBLNAME=?";
		return this.dao.getObject(sql,
				new Object[] { changeCode, tableName.trim() },
				ChangeTableBean.class);
	}

}
