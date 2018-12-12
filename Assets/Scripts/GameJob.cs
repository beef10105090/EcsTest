using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Google.Protobuf;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class GameJob : MonoBehaviour
{

    Socket _server;
    IPEndPoint _ip;
    byte[] _toSendData;
    int _num = 1000;
    [SerializeField]
    Transform tr1;
    [SerializeField]
    Transform tr2;

    [SerializeField]
    GameObject prefab;
    [SerializeField]
    GameObject jobPrefab;


    // Use this for initialization
    void Awake()
    {
        _trArray = new TransformAccessArray(1);
        _speedList = new NativeArray<float>(1, Allocator.TempJob);
        _dirList = new NativeArray<float>(1, Allocator.TempJob);
    }

    private void OnDestroy()
    {
        _handle.Complete();
        _trArray.Dispose();
        _speedList.Dispose();
        _dirList.Dispose();
    }

    int _fame;

    private void OnGUI()
    {
        GUI.TextField(new Rect(0, 0, 150, 40), _fame + "");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            //addTest(_num);
            addJobTest(_num);
        }
        _handle.Complete();
        float speed = UnityEngine.Random.Range(0.5f, 1);
        float dir = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
        _moveJob = new MoveJob()
        {
            SpeedList = _speedList,
            DirList = _dirList
        };
        _handle = _moveJob.Schedule(_trArray);
        JobHandle.ScheduleBatchedJobs();
        _fame = (int)(1 / Time.deltaTime);
    }

    MoveJob _moveJob;
    TransformAccessArray _trArray;
    JobHandle _handle;
    NativeArray<float> _speedList;
    NativeArray<float> _dirList;

    private void addJobTest(int num)
    {
        _handle.Complete();
        _trArray.capacity = _trArray.length + num;
        int length = _trArray.length + num;
        _speedList.Dispose();
        _dirList.Dispose();
        _speedList = new NativeArray<float>(length, Allocator.TempJob);
        _dirList = new NativeArray<float>(length, Allocator.TempJob);

        for (int i = 0; i < num; ++i)
        {
            GameObject go = Instantiate(jobPrefab);
            go.transform.position = new Vector3(UnityEngine.Random.Range(-10, 10),
                UnityEngine.Random.Range(-10, 10));
            _speedList[length - num + i] = UnityEngine.Random.Range(0.5f, 1);
            _dirList[length - num + i] = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
            _trArray.Add(go.transform);
        }
    }

    private void addTest(int num)
    {
        for (int i = 0; i < num; ++i)
        {
            GameObject go = Instantiate(prefab);
            go.transform.position = new Vector3(UnityEngine.Random.Range(-10, 10),
            UnityEngine.Random.Range(-10, 10));
        }
    }

    void jobTest4()
    {

        TransformAccessArray result = new TransformAccessArray(1);
        result.Add(transform);
        TransformJob tjob = new TransformJob
        {
            vec1 = tr1.transform.position,
            vec2 = tr2.transform.position
        };

        tjob.Schedule(result).Complete();

        result.Dispose();

    }

    void jobTest3()
    {
        NativeArray<float> a = new NativeArray<float>(2, Allocator.TempJob);

        NativeArray<float> b = new NativeArray<float>(2, Allocator.TempJob);

        NativeArray<float> result = new NativeArray<float>(2, Allocator.TempJob);

        a[0] = 1.1f;
        b[0] = 2.2f;
        a[1] = 3.3f;
        b[1] = 4.4f;

        MyParallelJob jobData = new MyParallelJob();
        jobData.a = a;
        jobData.b = b;
        jobData.result = result;

        // Schedule the job with one Execute per index in the results array and only 1 item per processing batch
        JobHandle handle = jobData.Schedule(result.Length, 1);

        // Wait for the job to complete
        handle.Complete();
        Debug.Log(result[0] + ", " + result[1]);
        // Free the memory allocated by the arrays
        a.Dispose();
        b.Dispose();
        result.Dispose();
    }

    void jobTest()
    {
        NativeArray<float> result = new NativeArray<float>(1, Allocator.TempJob);

        // Set up the job data
        MyJob jobData = new MyJob
        {
            a = 10,
            b = 10,
            result = result
        };

        // Schedule the job
        JobHandle handle = jobData.Schedule();

        // Wait for the job to complete
        handle.Complete();

        // All copies of the NativeArray point to the same memory, you can access the result in "your" copy of the NativeArray
        float aPlusB = result[0];
        Debug.Log(result[0]);
        // Free the memory allocated by the result array
        result.Dispose();

    }

    void jobTest2()
    {
        NativeArray<float> result = new NativeArray<float>(1, Allocator.TempJob);

        // Setup the data for job #1
        MyJob jobData = new MyJob();
        jobData.a = 10;
        jobData.b = 10;
        jobData.result = result;

        // Schedule job #1
        JobHandle firstHandle = jobData.Schedule();

        // Setup the data for job #2
        AddOneJob incJobData = new AddOneJob();
        incJobData.result = result;

        // Schedule job #2
        JobHandle secondHandle = incJobData.Schedule(firstHandle);

        // Wait for job #2 to complete
        secondHandle.Complete();

        // All copies of the NativeArray point to the same memory, you can access the result in "your" copy of the NativeArray
        float aPlusB = result[0];
        Debug.Log(result[0]);
        // Free the memory allocated by the result array
        result.Dispose();
    }

    private void netTest()
    {
        //Test.ContactBook tcb = new Test.ContactBook();
        //Test.Person tp = new Test.Person();
        //tp.Id = 1;
        //tp.Name = "小张";
        //Test.Phone tPhone = new Test.Phone();
        //tPhone.Type = Test.PhoneType.Home;
        //tPhone.Number = "111111111";
        //tp.Phones.Add(tPhone);
        //tPhone = new Test.Phone();
        //tPhone.Type = Test.PhoneType.Work;
        //tPhone.Number = "222222222";
        //tp.Phones.Add(tPhone);
        //tcb.Persons.Add(tp);

        //tp = new Test.Person();
        //tp.Id = 2;
        //tp.Name = "小王";
        //tPhone = new Test.Phone();
        //tPhone.Type = Test.PhoneType.Home;
        //tPhone.Number = "333333333";
        //tp.Phones.Add(tPhone);
        //tPhone = new Test.Phone();
        //tPhone.Type = Test.PhoneType.Work;
        //tPhone.Number = "444444444";
        //tp.Phones.Add(tPhone);
        //tcb.Persons.Add(tp);
        //byte[] bt = tcb.ToByteArray();
        //_toSendData = new byte[512];
        //uint length = (uint)bt.Length;
        //byte[] lengthData = BitConverter.GetBytes(1);
        //Array.Reverse(lengthData);
        //Array.Copy(bt, 0, _toSendData, 4, length);
        //Array.Copy(lengthData, _toSendData, 4);
        //byte[] bt = File.ReadAllBytes("/Users/Bacteria/go/src/UdpServer/test.txt");
        //for (int i = 0; i < bt.Length; ++i)
        //{
        //    Debug.Log(bt[i]);
        //}

        //IMessage cb = new Test.ContactBook();
        //Test.ContactBook p1;
        //p1 = (Test.ContactBook)cb.Descriptor.Parser.ParseFrom(bt);
        //for (int i = 0; i < p1.Persons.Count; ++i) {
        //    Debug.Log(p1.Persons[i].Id + ", " + p1.Persons[i].Name + ", " + p1.Persons[i].Phones[0].Type);
        //}

        Protobuf.CM_Login msg = new Protobuf.CM_Login();
        msg.Job = 1;
        byte[] bt = msg.ToByteArray();
        _toSendData = new byte[512];
        uint length = (uint)bt.Length;
        byte[] lengthData = BitConverter.GetBytes(1);
        Array.Reverse(lengthData);
        Array.Copy(bt, 0, _toSendData, 4, length);
        Array.Copy(lengthData, _toSendData, 4);

        _ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8001);

        //定义网络类型，数据连接类型和网络协议UDP
        _server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        Remote = (EndPoint)sender;
        Thread td = new Thread(new ThreadStart(Recieve));
        td.IsBackground = true;
        td.Start();
    }

    List<string> tdata = new List<string>();

    private void Recieve()
    {
        data = new byte[512];
        byte[] cdata;
        while (true)
        {
            int recv = _server.ReceiveFrom(data, ref Remote);
            lock (tdata)
            {
                cdata = new byte[recv - 4];
                Array.Copy(data, 4, cdata, 0, recv - 4);
                Protobuf.SM_RoleList list = DataParser.ParseData<Protobuf.SM_RoleList>(cdata);
                tdata.Add("myself:" + list.Myself);
                for (int i = 0; i < list.Players.Count; ++i)
                {
                    tdata.Add("players:" + list.Players[i]);
                }
            }
            Thread.Sleep(1);
        }


    }

    EndPoint Remote;
    byte[] data;
    // Update is called once per frame

}
