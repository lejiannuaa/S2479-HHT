package com.hola.bs.util;

import java.lang.reflect.Array;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.json.JSONArray;
import org.json.JSONObject;

import com.alibaba.fastjson.JSON;
import com.hola.bs.bean.JsonBean;
import com.hola.bs.json.detailVO.FirstCountDetail;

public class JsonUtil {
	
	public static Log log = LogFactory.getLog(JsonUtil.class);

	/**
	 * 解析json对象，将对象解析为JsonBean
	 * 
	 * @param json
	 *            json字符串
	 * @param node
	 *            每个需要解析的节点名称不包含config信息
	 * @return
	 */
	public static JsonBean jsonToList(String json, List<String> node)
			throws Exception {
		
		log.info("json:"+json);
		
		JsonBean bean = new JsonBean();
		JSONObject jsonObject = new JSONObject(json);
		System.out.println(jsonObject);
		JSONObject o = jsonObject.getJSONObject("root");
		HashMap<String, Map[]> data = new HashMap();
		int cnt = 0;
		for (String s : node) {
			JSONArray jsons = o.getJSONArray(s);
			Map m[] = new HashMap[jsons.length()];
			for (int i = 0; i < jsons.length(); i++) {
				Iterator keys = jsons.getJSONObject(i).keys();
				// System.out.println(jsons.getJSONObject(i));
				m[i] = new HashMap();
				while (keys.hasNext()) {
					String key = String.valueOf(keys.next());
					// System.out.println("key:"+key+" value:"+jsons.getJSONObject(i).getString(key));
					m[i].put(key, jsons.getJSONObject(i).getString(key));
				}
			}
			data.put(s, m);
			cnt++;
		}
		bean.setData(data);
		o = o.getJSONObject("config");
		bean.setId(o.getString("id"));
		bean.setDirection(o.getString("direction"));
		bean.setType(o.getString("type"));

		return bean;
	}

	/**
	 * 采用fastjson的面向对象方式解析json对象
	 * @param json JSON参数
	 * @return
	 * author: S2139
	 * 2012 Aug 24, 2012 10:48:48 AM
	 */
	public static com.alibaba.fastjson.JSONObject analyze(String json){
		log.info("json:"+json);
		com.alibaba.fastjson.JSONObject jsonObject = ((com.alibaba.fastjson.JSONObject)JSON.parse(json)).getJSONObject("root");
		
		return jsonObject;
	}
	
	/**
	 * 从现有的jsonObject中获取指定key下的JSON信息，包装成相应的对象
	 * @param jsonObject 原先解析出来的类型为JSONObjec的对象 
	 * @param key
	 * @param clazz 要转换成的对象类型
	 * @return 相应的对象。如果key属性不存在，返回空
	 * author: S2139
	 * 2012 Aug 24, 2012 10:48:48 AM
	 */
	public static Object getObject(com.alibaba.fastjson.JSONObject jsonObject,String key, Class clazz){
		if(jsonObject.containsKey(key)){
			return jsonObject.getObject(key, clazz);
		}else{
			return null;
		}
	}
	
	/**
	 * 从json对象中获取config参数下的信息
	 * @param jsonObject
	 * @param config 一个Config对象，指定config信息的封装形式
	 * @return Config对象。如果没有，返回null
	 * *author: S2139
	 * 2012 Aug 24, 2012 10:48:48 AM
	 */
	public static com.hola.bs.json.Config getConfig(com.alibaba.fastjson.JSONObject jsonObject,com.hola.bs.json.Config config){
		Object o = getObject(jsonObject,"config",config.getClass());
		if(o != null){
			return (com.hola.bs.json.Config)o;
		}else{
			return null;
		}
	}
	
	
	/**
	 * 获取JSON信息中明细部分的业务信息
	 * @param <T>
	 * @param jsonObject
	 * @param clazz 指定的对象类型封装
	 * @return
	 */
	public static <T>T[] getDetail(com.alibaba.fastjson.JSONObject jsonObject,Class<T> clazz){
		String detail = "detail";
		if(jsonObject.containsKey(detail)){
			com.alibaba.fastjson.JSONArray jSonArray = (com.alibaba.fastjson.JSONArray)jsonObject.get(detail);
			T[] ar = (T[])Array.newInstance(clazz, jSonArray.size());
			for(int i=0; i<jSonArray.size(); i++){
				ar[i] = jSonArray.getObject(i, clazz);
			}
			return ar;
		}else{
			return null;
		}
	}
	
	public String toJson(Object o) {
		return new JSONObject(o).toString();
	}

	/**
	 * @param args
	 * @throws Exception
	 */
	public static void main(String[] args) throws Exception {
		List l = new ArrayList();
		// l.add("diff");
		l.add("info");
		String s1 = "{\"root\":{\"config\":{\"type\":\"0\",\"direction\":\"1\",\"id\":\"00202\"},\"info\":[{\"SKU\":\"10000\",\"num\":\"10\"},{\"SKU\":\"20000\",\"num\":\"20\"}],\"diff\":[{\"SKU\":\"10000\",\"reason\":\"1\",\"count\":\"1\"},{\"SKU\":\"20000\",\"reason\":\"2\",\"count\":\"2\"}]}}";
		String s2 = "{\"root\":{\"config\":{\"type\":\"12\",\"direction\":\"ss\",\"id\":\"1\"},\"info\":{\"SKU\":\"2445\",\"完好数量\":\"12\"},\"diff\":{\"SKU\":\"22309\",\"reason\":\"01\",\"count\":\"12\"}}}";
		
		String s3 = "{\"root\":{\"config\":{\"type\":\"0\",\"direction\":\"1\",\"id\":\"00202\"},\"info\":{\"}}";
//		String s3 = "{\"root\":{\"config\":{\"type\":\"0\",\"direction\":\"1\",\"id\":\"00202\"},\"info\":[{\"SKU\":\"10000\",\"完好数量\":\"10\"},{\"SKU\":\"20000\",\"num\":\"20\"}],\"diff\":[{\"SKU\":\"10000\",\"reason\":\"1\",\"count\":\"1\"},{\"SKU\":\"20000\",\"reason\":\"2\",\"count\":\"2\"}]}}";
		System.out.println(s1);
		JsonUtil.jsonToList(s1, l);
	}

}
