#include "stack.h"
#include "Queue.h"
#include "LinkedList.h"
#include "LinkedStack.h"
#include "LinkedQueue.h"
#include <iostream>
#include <string>

using namespace std;

int main() {

    string input("");
    int numInput;
    int size;
    int index;

    while (true){

        cout << "Welcome To DS Controls" << endl;
        cout << "Type what you want to try" << endl << endl;
        cout << "s  : stack control" << endl;
        cout << "q  : queue control" << endl;
        cout << "l  : linked list control" << endl;
        cout << "ls : linked stack control" << endl;
        cout << "lq : linked queue control" << endl;
        cout << "t  : tree control" << endl;
        cout << "ex : end program" << endl;

        getline(cin, input);

        if (input == "s"){
            cout << "You choose stack. PLZ input size. >> ";
            
            cin >> size;
            cin.ignore();
            Stack myStack(size);

            while (true){
                cout << "push | pop | print | top | reverse | count | quit >> ";
                cin >> input;

                if (input == "push"){
                    cin >> numInput;
                    if (myStack.isFull()) cout << "Stack is Full!" << endl;
                    else myStack.push(numInput); 
                }
                else if (input == "pop"){
                    cout << myStack.pop() << " is popped" << endl; 
                }
                else if (input == "print"){
                    myStack.printAll(); 
                }
                else if (input == "reverse"){
                    if (myStack.isEmpty()) cout << "Stack is Empty!" << endl;
                    else {
                        myStack.reverseStack(); 
                        cout << "complete" << endl;
                    }
                }
                else if (input == "top"){
                    if (myStack.isEmpty()) cout << "Stack is Empty!" << endl;
                    else cout << myStack.topNum() << " is top" << endl;; 
                }
                else if (input == "count"){
                    if (myStack.isEmpty()) cout << "Stack is Empty!" << endl;
                    cout << "Stack has " << myStack.countNum() << " Numbers" << endl;
                }

                else if (input == "quit"){
                    cout << "Stack quit" << endl;
                    break;
                }
                else{
                    cout << "input invalid PLZ try again" << endl;
                }
            }
        } 
        else if (input == "q"){
            cout << "You choose queue. PLZ input size. >> ";
            
            cin >> size;
            cin.ignore();
            Queue myQueue(size+1);

            while (true){
                cout << "push | pop | print | high | reverse | count | quit >> ";
                cin >> input;

                if (input == "push"){
                    cin >> numInput;
                    if (myQueue.isFull()) cout << "Queue is Full!" << endl;
                    myQueue.push(numInput); 
                }
                else if (input == "pop"){
                    if (myQueue.isEmpty()) cout << "Queue is Empty!" << endl;
                    else cout << myQueue.pop() << " is popped" << endl; 
                }
                else if (input == "print"){
                    if (myQueue.isEmpty()) cout << "Queue is Empty!" << endl;
                    else myQueue.printAll(); 
                }
                else if (input == "reverse"){
                    if (myQueue.isEmpty()) cout << "Queue is Empty!" << endl;
                    else{
                        myQueue.reverseQueue(); 
                        cout << "complete" << endl;
                    }
                }
                else if (input == "high"){
                    if (myQueue.isEmpty()) cout << "Queue is Empty!" << endl;
                    else cout << myQueue.highestNum() << " is highest" << endl;; 
                }
                else if (input == "count"){
                    if (myQueue.isEmpty()) cout << "Queue is Empty!" << endl;
                    else cout << "Queue has " << myQueue.countNum() << " Numbers" << endl;
                }

                else if (input == "quit"){
                    cout << "Queue quit" << endl;
                    break;
                }
                else{
                    cout << "input invalid PLZ try again" << endl;
                }
            }
        }
        else if (input == "l"){
            cout << "You choose Linked List" << endl;

            LinkedList myLinkedList;
            node temp;

            while (true){
                cout << "head_Add | tail_Add | del_Head | del_Tail | insert | count | del_at | search | print | quit >> ";
                cin >> input;

                if (input == "head_Add"){
                    cin >> numInput;
                    temp.setData(numInput);
                    myLinkedList.addToHead(temp);
                    cout << numInput << " is Added to Head" << endl;
                }
                else if (input == "tail_Add"){
                    cin >> numInput;
                    temp.setData(numInput);
                    myLinkedList.addToTail(temp);
                    cout << numInput << " is Added to Tail" << endl;
                }
                else if (input == "del_Head"){
                    temp = myLinkedList.deleteHead();
                    
                }
                else if (input == "del_Tail"){
                    temp = myLinkedList.deleteTail();
                    
                }
                else if (input == "insert"){
                    cin >> numInput >> index;
                    temp.setData(numInput);
                    myLinkedList.insertAt(index, temp);
                    
                }
                else if (input == "del_at"){
                    cin >> index;
                    temp = myLinkedList.deleteAt(index);
                    
                }
                else if (input == "count"){
                    cout << myLinkedList.countNode() << " nodes are in it" << endl; 
                }
                else if (input == "search"){
                    cin >> index;
                    temp = myLinkedList.searchNode(index);
                     
                }
                else if (input == "print"){
                    myLinkedList.printAll();
                }

                else if (input == "quit"){
                    cout << "Linked List quit" << endl;
                    break;
                }
                else{
                    cout << "input invalid PLZ try again" << endl;
                }
            }
        }
        else if (input == "ls"){
            
            cout << "You choose Linked stack.";
            LinkedStack MyLinkedStack;
            node temp;

            while (true){
                cout << "push | pop | print | reverse | count | quit >> ";
                cin >> input;

                if (input == "push"){
                    cin >> numInput;
                    temp.setData(numInput);
                    MyLinkedStack.push(temp); 
                }
                else if (input == "pop"){
                    MyLinkedStack.pop();
                }
                else if (input == "print"){
                    MyLinkedStack.printAll(); 
                }
                else if (input == "reverse"){
                MyLinkedStack.reverseStack(); 
                        cout << "complete" << endl;
                }
                else if (input == "count"){
                    cout << "Stack has " << MyLinkedStack.countNum() << " Numbers" << endl;
                }

                else if (input == "quit"){
                    cout << "Linked Stack quit" << endl;
                    break;
                }
                else{
                    cout << "input invalid PLZ try again" << endl;
                }
            }
        }
        else if (input == "lq"){
            cout << "You choose Linked Queue.";

            LinkedQueue MyLinkedQueue;
            node temp;
        }
        else if (input == "ex"){
            cout << "program ended bye!";
            break;
        }
    }
    return 0;
}
