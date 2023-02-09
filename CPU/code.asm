    mov     $1, 255
loop:
    inc     $0
    cmp     $0, $1
    je      end
    jump    loop
end:
    jump    end