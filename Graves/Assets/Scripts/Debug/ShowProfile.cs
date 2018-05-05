using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Graves
{
    public class ShowProfile : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            uint monoUsed = Profiler.GetMonoUsedSize();
            uint monoSize = Profiler.GetMonoHeapSize();
            uint totalUsed = Profiler.GetTotalAllocatedMemory(); // == Profiler.usedHeapSize
            uint totalSize = Profiler.GetTotalReservedMemory();

            string txt = string.Format(
                "mono:{0}/{1} kb({2:f1}%)\n" +
                "total:{3}/{4} kb({5:f1}%)",
                monoUsed / 1024, monoSize / 1024, 100.0 * monoUsed / monoSize,
                totalUsed / 1024, totalSize / 1024, 100.0 * totalUsed / totalSize);

            gui_debug_window.main.drawLine(txt);
            gui_debug_window.main.drawLine("fps:"+(1f/Time.deltaTime).ToString("F2"));
        }
    }
}