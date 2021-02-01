using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NeuralNetworks
{
    public static class ExtensionMethods
    {
        public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
        {
            if (ReferenceEquals(self, other))
                return;
            
            foreach (var from in self)
            {
                foreach (var to in other)
                {
                    if (from.Out == null)
                        continue;
                    from.Out.Add(to);
                    if (to.In == null)
                        continue;
                    to.In.Add(from);
                }
            }
        }
    }
    public class Neuron : IEnumerable<Neuron>
    {
        public float value;
        public List<Neuron> In, Out;
        
        public IEnumerator<Neuron> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class NeuronLayer : Collection<Neuron>
    {
        
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            // unite Neuron and NeuronLayer in IEnumerable<Neuron> Composite Class
            // Add an Extension Method for IEnumerable<Neuron>
            // We will be able to connect a single node to the group of nodes (layer)
            var neuron1 = new Neuron();
            var neuron2 = new Neuron();
            neuron1.ConnectTo(neuron2);
            var layer1 = new NeuronLayer();
            var layer2 = new NeuronLayer();
            neuron1.ConnectTo(layer1);
            layer1.ConnectTo(neuron2);
        }
    }
}