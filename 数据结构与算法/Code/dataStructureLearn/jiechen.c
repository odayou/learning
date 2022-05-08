//
// Created by tommy on 2022/4/22.
//
#include<stdio.h>
long factorial(int n);
void main()
{
    int num;
    for (int num = 0; num < 10; ++num) {
        printf("%d!=%ld\n", num, factorial(num));
    }
}
long factorial (int n) {
    if(n == 0)
    {
        return 1;
    }
    return n* factorial(n-1);
}