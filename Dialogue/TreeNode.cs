using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    public TreeNode leftNode, rightNode;
    private int nodeValue = 0;
    public DialogueText dialogueText;
    
    public TreeNode(int nodeValue, DialogueText dialogueText){
        this.nodeValue = nodeValue;
        this.dialogueText = dialogueText;
    }
    
    public void treeTraversal(TreeNode node){
        if(node == null) return;
        if(node.leftNode != null) treeTraversal(node.leftNode);
        if(node.rightNode != null) treeTraversal(node.rightNode);
    }
    
    public void addNode(TreeNode root, TreeNode node){
        if(root.nodeValue < node.nodeValue){
            if(root.rightNode == null) root.rightNode = node;
            else addNode(root.rightNode, node);
        }
        else{
            if(root.leftNode == null) root.leftNode = node;
            else addNode(root.leftNode, node);
        }
    }
    
    public int getValue(){
        return this.nodeValue;
    }
    
    public string getDialogueText(){
        return this.dialogueText.text;
    }
}
