package com.hola.bs.impl;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.easymock.EasyMock;
import org.junit.Assert;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.powermock.api.easymock.PowerMock;
import org.powermock.core.classloader.annotations.PrepareForTest;
import org.powermock.modules.junit4.PowerMockRunner;

import com.hola.bs.util.SpringUtil;

@RunWith(PowerMockRunner.class)
@PrepareForTest({SpringUtil.class})  
public class HHT_201_004Test {

    @Test
    public void testProcessString() {
        
        List mockList = new ArrayList<Map>();
        Map mockMap = new HashMap();
        mockMap.put("PO单号", "PO单号_001");
        mockMap.put("厂商编号", "厂商编号_001");
        mockMap.put("厂商名称", "厂商名称_001");
        mockMap.put("收货状态", "收货状态_001");
        mockList.add(mockMap);
        
        PowerMock.mockStatic(SpringUtil.class); 
        // mock info 
        String sql = SqlConfig.get("HHT_201_004_info");
        EasyMock.expect(SpringUtil.searchForList(sql)).andReturn(mockList); 
        PowerMock.replay(SpringUtil.class);  
        
        
        ProcessUnit unit = new HHT_201_004();
        
        String request = "request=004;usr=roy;op=01;bc=b0001;from=20120101;to=20120201;state=1;opusr=roy";
        
        String res = unit.process(null);
        PowerMock.verify(SpringUtil.class);
        
        Response response = new Response();
        response.setResponse("004");
        response.setCode(Response.SUCCESS);
        
        Assert.assertEquals(response.toString(), res);
        
    }
 
}
