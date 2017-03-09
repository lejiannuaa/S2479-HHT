package com.hola.tarjda.cache;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import com.hola.common.SpringContextHelper;
import com.hola.common.dao.HhtserverDao;
import com.hola.common.exception.BscfgException;
import com.hola.common.model.mw.Chginfo;
import com.hola.tarjda.service.TarJdaService;

/**
 * 缓存配置信息(注意：只适合bs上传jda的配置)
 * @author 王成(chengwangi@isoftstone.com)
 * @date 2012-12-28 下午5:23:27
 */
public class ConfigCache 
{
	private		static		ConfigCache		configCache;
	private		final		Map<String, Chginfo>	chginfoMap = 
							new HashMap<String, Chginfo>();
	private		final		TarJdaService service = 
							(TarJdaService) SpringContextHelper.getInstance().getBean("tarJdaService");
	private		final		HhtserverDao	hhtserverDao = 
							(HhtserverDao)SpringContextHelper.getInstance().getBean("hhtserverDao");
	
	private		final		Map<String , String >		allChgcodeStrMap = 
							new		HashMap<String, String>();
	private ConfigCache()
	{}
	
	public synchronized static ConfigCache getInstance()
	{
		if(configCache == null)
			configCache = new ConfigCache();
		
		return configCache;
	}
	/**
	 * 从缓存中获取配置信息
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2012-12-28 下午5:27:46
	 * @param chgcode
	 * @return
	 * @throws BscfgException
	 */
	public Chginfo getChginfoForChgcode(String chgcode) throws BscfgException
	{
		if(chginfoMap.containsKey(chgcode) == false)
		{
			Chginfo chginfo = service.loadSrcConfig(chgcode);
			chginfoMap.put(chgcode, chginfo);
		}
		return chginfoMap.get(chgcode);
	}
	/**
	 * 获取对应交换类型的所有chgcode字符串
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-16 下午3:17:32
	 * @param ioType
	 * @return
	 */
	public String getAllChgcodeStrForIOType(String ioType)
	{
		if(this.allChgcodeStrMap.containsKey(ioType) == false)
		{
			StringBuffer totle = new StringBuffer();
			List<String> chgcodes = hhtserverDao.getAllChgcodeByIOType(ioType);
			boolean isFirst = true;
			for (String chgcode : chgcodes) 
			{
				if(isFirst == false)
					totle.append(",");
				isFirst = false;
				totle.append("'").append(chgcode).append("'");
			}
			this.allChgcodeStrMap.put(ioType, totle.toString());
		}
		return this.allChgcodeStrMap.get(ioType);
	}
	/**
	 * 清理缓存
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2012-12-28 下午5:27:39
	 */
	public void clearCache()
	{
		chginfoMap.clear();
		this.allChgcodeStrMap.clear();
	}
}
