PaintFullScreen:
    mov     $0, 128
    mov     $1, 5
    shl     $0, $1 ; $0 = 4096
    mov     $3, 255
    mov     $4, 8
    shl     $3, $4 ; $3 = mt grande
    mov     $5, 255
    add     $3, $5 ; $3 = 1111 1111 1111 1111
    mov     [$0], $3 ; 4096 = 11111 11111 1 111 1 11 1 11 1 1 1 1 1 1 1 
    loop:    
        inc $0;
        mov [$0], $3
        jmp loop
