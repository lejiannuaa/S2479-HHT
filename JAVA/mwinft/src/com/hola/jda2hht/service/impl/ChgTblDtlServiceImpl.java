package com.hola.jda2hht.service.impl;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import com.hola.jda2hht.dao.BaseDao;
import com.hola.jda2hht.model.ChangeTableDetailBean;
import com.hola.jda2hht.service.IChgTblDtlService;

public class ChgTblDtlServiceImpl implements IChgTblDtlService {
	private BaseDao dao;

	public BaseDao getDao() {
		return dao;
	}

	public void setDao(BaseDao dao) {
		this.dao = dao;
	}

	@SuppressWarnings("unchecked")
	@Override
	public Map<String, List<ChangeTableDetailBean>> getAllTableDetail(
			String tableId) {
		Map<String, List<ChangeTableDetailBean>> result = new LinkedHashMap<String, List<ChangeTableDetailBean>>();
		String sql = "select ID,TBLID,SRCLAYCODE,SRCFLDCODE,SRCSEQ,TARLAYCODE,TARFLDCODE,TARSEQ,ISSTR,ISSEQ from HOLA_APP_CFG_CHGTBLDTL where TBLID=? order by SRCLAYCODE,SRCSEQ";
		List<ChangeTableDetailBean> list = (List<ChangeTableDetailBean>) this.dao
				.getList(sql, new Object[] { tableId },
						ChangeTableDetailBean.class);

		for (ChangeTableDetailBean bean : list) {
			List<ChangeTableDetailBean> beanList = result.get(bean
					.getSrclaycode());
			if (beanList == null) {
				beanList = new ArrayList<ChangeTableDetailBean>();
				result.put(bean.getSrclaycode(), beanList);
			}
			beanList.add(bean);
		}
		return result;
	}

	@Override
	public ChangeTableDetailBean getDetailBean(String id, String isstr) {
		String sql = "select ID,TBLID,SRCLAYCODE,SRCFLDCODE,SRCSEQ,TARLAYCODE,TARFLDCODE,TARSEQ,ISSTR,ISSEQ from HOLA_APP_CFG_CHGTBLDTL where TBLID=? and ISSTR=?";
		return this.dao.getObject(sql, new Object[] { id, isstr },
				ChangeTableDetailBean.class);
	}
}
