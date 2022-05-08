#include <stdio.h>
#include <math.h>

void IsPrime ()
{
    printf("正整数...");
    int m = 0;
    scanf("%d", &m);
    for (int i = 2; i <= sqrt(m); i++) {
        int r = m % i;
        if (r == 0) {
            printf("not 质数");
            return;
        }
    }
    printf("质数 %d", m);
}
//
//int main() {
//    printf("Hello, World!\n");
//    IsPrime();
//    return 0;
//}

