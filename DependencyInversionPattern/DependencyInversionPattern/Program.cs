using System;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInversionPattern
{
    public enum Relationship
    {
        Parent,
        Child,
        Sibling
    }
    public class Person
    {
        public string Name;
    }

    public interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string parentName);
    }
    
    public class Relation : IRelationshipBrowser
    {
        private List<(Person, Relationship, Person)> relations =
            new List<(Person, Relationship, Person)>();

        public void AddParentAndChild(Person parent, Person child)
        {
            relations.Add((parent,Relationship.Parent,child));
            relations.Add((child,Relationship.Parent,parent));
        }
        
        //BEFORE
        public List<(Person, Relationship, Person)> Relations => relations;
        
        //AFTER
        public IEnumerable<Person> FindAllChildrenOf(string parentName)
        {
            return relations.Where(relation => relation.Item1.Name == parentName
                                               && relation.Item2 == Relationship.Parent
            ).Select(relation => relation.Item3);
        }
    }

    //BEFORE
    public class Research
    {
        public void FindChildrenOf(Relation relationships,string parentName)
        {
            foreach (var relation in relationships.Relations)
            {
                if (relation.Item1.Name == parentName && relation.Item2 == Relationship.Parent)
                {
                    Console.WriteLine($"{relation.Item1.Name} has child {relation.Item3.Name}");
                }
            }
        }
    }
    
    // AFTER
    public class NewResearch
    {
        public void FindChildrenOf(IRelationshipBrowser relationships, string parentName)
        {
            foreach (var child in relationships.FindAllChildrenOf(parentName))
            {
                Console.WriteLine($"{parentName} has child {child.Name}");
            }
        }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            Person parent = new Person {Name = "John"};
            Person child1 = new Person {Name = "Nick"};
            Person child2 = new Person {Name = "Hannah"};
            
            var relationships = new Relation();
            relationships.AddParentAndChild(parent,child1);
            relationships.AddParentAndChild(parent,child2);
            
            //BEFORE
            new Research().FindChildrenOf(relationships, "John");
            
            //AFTER
            new NewResearch().FindChildrenOf(relationships,"John");
            
        }
    }
}