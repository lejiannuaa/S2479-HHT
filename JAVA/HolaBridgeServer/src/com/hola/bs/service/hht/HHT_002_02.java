package com.hola.bs.service.hht;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.UUID;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.bean.JsonBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.JsonUtil;
import com.hola.bs.util.Root;

/**
 * 調撥收貨中，提交確認收貨商品的操作
 * 
 * @author s1713 modify by s2139S
 * 
 */
@Transactional(propagation = Propagation.REQUIRED, rollbackFor = Exception.class)
public class HHT_002_02 extends BusinessService implements ProcessUnit {
	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			processData(bean);
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
			log.error("", e);
			throw new RuntimeException(e);
		}
		// log.info("response=" + bean.getResponse().toString());
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht002.02確認調撥收貨地詳細情況, response="
				+ bean.getResponse().toString());
		// System.out.println("001_01:response="+response.toString());
		System.out.println("返回的结果是：===================》"
				+ bean.getResponse().toString());
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) {

		String bc = bean.getRequest().getParameter(
				configpropertyUtil.getValue("bc"));
		List<Map> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil
				.getValue(bean.getUser().getStore(), "hht002.02"),
				new Object[] { bc });

		Root r = new Root();
		if (list.size() < 1) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.01"));
		} else {
			String s = String.valueOf(list.get(0).get("HHTSTA"));
			if (s == null || !"1".equals(s.trim())) {
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc(
						configpropertyUtil.getValue("msg.02"));
			} else {
				// String sysId = bean.getUser() != null
				// ? bean.getUser().getStore() + this.systemUtil.getSysid()
				// : this.systemUtil.getSysid();
				// String username = bean.getUser() != null
				// ? bean.getUser().getName()
				// : "s1777";
				String username = "";
				String sysId = "";
				if (bean.getUser() != null) {
					username = bean.getUser().getName();
					sysId = bean.getUser().getStore()
							+ this.systemUtil.getSysid();
				} else {
					try {
						throw new Exception("当前用户已登出，或系统异常，找不到操作用户");
					} catch (Exception e) {
					}
				}
				String hhtflc = list.get(0).get("HHTFLC").toString();
				String hhttlc = list.get(0).get("HHTTLC").toString();
				List l = new ArrayList();
				l.add(configpropertyUtil.getValue("hht001nodeName.01"));
				l.add(configpropertyUtil.getValue("hht001nodeName.02"));
				JsonBean json = null;
				try {
					json = JsonUtil.jsonToList(
							String.valueOf(bean.getRequest().getParameter(
									configpropertyUtil.getValue("xml"))), l);
				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}

				int sqlSize = 1;
				if (json.getData().get(
						configpropertyUtil.getValue("hht001nodeName.01")) != null)
					sqlSize += json.getData().get(
							configpropertyUtil.getValue("hht001nodeName.01")).length;
				if (json.getData().get(
						configpropertyUtil.getValue("hht001nodeName.02")) != null)
					sqlSize += json.getData().get(
							configpropertyUtil.getValue("hht001nodeName.02")).length;
				int cnt = 0;
				String sql[] = new String[sqlSize];
				Object o[] = new Object[sqlSize];
				if (json.getData().get(
						configpropertyUtil.getValue("hht001nodeName.01")) != null) {
					for (Map m : json.getData().get(
							configpropertyUtil.getValue("hht001nodeName.01"))) {
						sql[cnt] = sqlpropertyUtil.getValue(bean.getUser()
								.getStore(), "hht002.02.01");
						o[cnt] = new Object[] { sysId,
								m.get(configpropertyUtil.getValue("whsl")), bc,
								m.get(configpropertyUtil.getValue("nodeSKU")) };

						cnt++;
					}
				}
				if (json.getData().get(
						configpropertyUtil.getValue("hht001nodeName.02")) != null) {
					for (Map m : json.getData().get(
							configpropertyUtil.getValue("hht001nodeName.02"))) {

						sql[cnt] = sqlpropertyUtil.getValue(bean.getUser()
								.getStore(), "hht002.02.02");
						o[cnt] = new Object[] {
								sysId,
								bc,
								m.get(configpropertyUtil.getValue("reason")),
								m.get(configpropertyUtil.getValue("nodeSKU")),
								m.get(configpropertyUtil.getValue("nodecount")),
								hhtflc, hhttlc };
						// bean.getUser().getAttribute("HHTFLC"),
						// bean.getUser().getAttribute("HHTTLC")};

						cnt++;
					}
				}
				sql[sqlSize - 1] = sqlpropertyUtil.getValue(bean.getUser()
						.getStore(), "hht002.02.03");
				o[sqlSize - 1] = new Object[] { sysId, username, bc };

				// 查询是否存在sku，如果存在，则更新数量，如果不存在，则新增数据
				sqlSize = json.getData().get(
						configpropertyUtil.getValue("hht001nodeName.01")).length;
				String sql_Htob1f[] = new String[sqlSize];
				Object o_Htob1f[] = new Object[sqlSize];
				cnt = 0;
				for (Map m : json.getData().get(
						configpropertyUtil.getValue("hht001nodeName.01"))) {
					String sku = m.get(configpropertyUtil.getValue("nodeSKU"))
							.toString();

					List<Map> list3 = jdbcTemplateUtil.searchForList(
							sqlpropertyUtil.getValue(bean.getUser().getStore(),
									"hht002.02.07"), new Object[] { sku });

					if (list3.size() == 0) {
						sql_Htob1f[cnt] = sqlpropertyUtil.getValue(bean
								.getUser().getStore(), "hht002.02.05");
						o_Htob1f[cnt] = new Object[] { sysId,
								bean.getUser().getStore(), sku, m.get("whsl") };

					} else {
						List<Map> list2 = jdbcTemplateUtil
								.searchForList(
										sqlpropertyUtil.getValue(bean.getUser()
												.getStore(), "hht002.02.04"),
										new Object[] {
												bean.getRequest()
														.getParameter(
																configpropertyUtil
																		.getValue("bc")),
												sku });

						sql_Htob1f[cnt] = sqlpropertyUtil.getValue(bean
								.getUser().getStore(), "hht002.02.06");
						
						o_Htob1f[cnt] = new Object[] {
								m.get("whsl"), sku };
						/*
						o_Htob1f[cnt] = new Object[] {
								list2.get(0).get("hhtpqt"), sku };
								*/
					}

					cnt++;
				}
				this.jdbcTemplateUtil.update(sql_Htob1f, o_Htob1f);

				String guid = bean.getRequest().getParameter(
						configpropertyUtil.getValue("guid"));
				String requestValue = bean.getRequest().getParameter(
						configpropertyUtil.getValue("requestValue"));
				// 原业务处理相关的sql执行
				System.out.println("guid = " + guid);
				super.update(bean.getRequest().getRequestID(), sysId, sql, o,
						bean.getUser().getStore(), guid, requestValue, "");

				// 下面是phase3，写RC差异接口表数据
				// 根据当前登录帐号usrNo获取该用户中文名usrName
				String usrName = "";
				String usrNo = bean.getRequest().getParameter(
						configpropertyUtil.getValue("usr"));
				List<Map> usrlist = jdbcTemplateUtil.searchForList(
						sqlpropertyUtil.getValue("server", "hht002.02.08"),
						new Object[] { usrNo });
				if (usrlist.size() >= 1) {
					usrName = String.valueOf(usrlist.get(0).get("user_cname"));
				}
				// 取一个新的instno，供写RC差异接口表，新的chgcode HHT_TODMS
				sysId = bean.getUser().getStore() + this.systemUtil.getSysid();

				// 根据箱号bc取得波次号btNo
				Object btNo = null;
				List<Map> btNoList = jdbcTemplateUtil.searchForList(
						sqlpropertyUtil.getValue(bean.getUser().getStore(),
								"hht002.02.10"), new Object[] { bc });
				if (btNoList.size() >= 1) {
					btNo = String.valueOf(usrlist.get(0).get("hhtwav"));
				}

				// 根据Json对象中diff节点下原因个数，产生插入DMS_RCDIFF表的SQL及参数
				int diffReasonSQLSize = 0;
				// 取得Json对象中diff节点下差异原因的个数
				diffReasonSQLSize = json.getData().get(
						configpropertyUtil.getValue("hht001nodeName.02")).length;
				String sql_diffReason[] = new String[diffReasonSQLSize];
				Object o_diffReason[] = new Object[diffReasonSQLSize];

				// 从参数中取整箱的照片
				String cphoto = bean.getRequest().getParameter(
						configpropertyUtil.getValue("nodePhoto"));

				cnt = 0;
				Map[] diffReason = json.getData().get(
						configpropertyUtil.getValue("hht001nodeName.02"));
				// 遍历diff节点下所有的原因
				if (diffReason != null && diffReason.length > 0) {
					for (Map mReason : diffReason) {
						Object sku = mReason.get(configpropertyUtil
								.getValue("nodeSKU"));
						Object reson = mReason.get(configpropertyUtil
								.getValue("nodeReason"));
						Object diffQTY = mReason.get(configpropertyUtil
								.getValue("nodecount"));
						Object photo = mReason.get(configpropertyUtil
								.getValue("nodePhoto"));
						Object skuName = "";
						Object chsl = 0;
						Object whsl = 0;

						// 遍历info节点，根据sku号找到该sku的品名，出货数量，完好数量信息
						for (Map minfo : json.getData().get(
								configpropertyUtil
										.getValue("hht001nodeName.01"))) {
							Object infosku = minfo.get(configpropertyUtil
									.getValue("nodeSKU"));
							if (sku.equals(infosku)) {
								List<Map> skuNameList = jdbcTemplateUtil
										.searchForList(sqlpropertyUtil
												.getValue(bean.getUser()
														.getStore(),
														"hht002.02.11"),
												new Object[] { sku });
								if (skuNameList.size() >= 1) {
									skuName = String.valueOf(skuNameList.get(0)
											.get("HHTDSC"));
								}
								List<Map> chslList = jdbcTemplateUtil
										.searchForList(sqlpropertyUtil
												.getValue(bean.getUser()
														.getStore(),
														"hht002.02.12"),
												new Object[] { bc, sku });
								if (chslList.size() >= 1) {
									chsl = String.valueOf(chslList.get(0).get(
											"HHTPQT"));
								}

								whsl = minfo.get(configpropertyUtil
										.getValue("nodeWhsl"));
							}
						}
						
						//从JDAID2HF里读取HHTFLC，如果为13196则不插入hhtrcdiff
						List<Map> jdalist = jdbcTemplateUtil
								.searchForList(sqlpropertyUtil
										.getValue(bean.getUser()
												.getStore(),
												"hht002.02.13"),
										new Object[] {bc});
						
						if(jdalist.size()>0){
							sql_diffReason[cnt] = sqlpropertyUtil.getValue(bean
									.getUser().getStore(), "hht002.02.09");
							o_diffReason[cnt] = new Object[] { sysId,
									bean.getUser().getStore(), btNo, bc, sku,
									skuName, chsl, whsl, diffQTY, reson,
									photo + "," + cphoto, usrNo, usrName };
							cnt++;
						}
						
					}
					// 上面是phase3，准备RC差异接口表数据的sql

					// 执行RC差异接口表sql,模拟一个新的command
					// ID,以便在sql.properties为其配置chgcode

					UUID uuid = UUID.randomUUID();// 为防止写hht_request_record出现主键重复，新产生一个guid
					guid = uuid.toString().replace("-", "");

					System.out.println("guid = " + guid);
					super.update("002_06", sysId, sql_diffReason, o_diffReason,
							bean.getUser().getStore(), guid, requestValue, "");
				}
			}

		}

	}
}
