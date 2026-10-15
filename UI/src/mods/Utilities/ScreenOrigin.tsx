import { ReactNode, useEffect, useRef, useState } from "react";

// Some slots (e.g. "Menu") are rendered inside a container that doesn't start at the screen's
// top-left corner. ScreenOrigin measures that container and moves its children back to (0, 0),
// so absolute left/top values inside it are screen coordinates again.
export const ScreenOrigin = ({ children }: { children: ReactNode }) => {
    const ref = useRef<HTMLDivElement>(null);
    const [offset, setOffset] = useState({ x: 0, y: 0 });

    useEffect(() => {
        // Gameface lays the UI out a few frames after mount (rects read 0 at first),
        // so keep measuring each frame until the container's position is stable.
        let frame = 0;
        let stableFrames = 0;
        let last = { x: NaN, y: NaN };

        const measure = () => {
            const container = ref.current?.offsetParent;
            if (container) {
                const rect = container.getBoundingClientRect();
                const next = { x: -rect.left, y: -rect.top };
                stableFrames = (next.x === last.x && next.y === last.y) ? stableFrames + 1 : 0;
                if (stableFrames === 0) setOffset(next);
                last = next;
            }
            if (stableFrames < 10) frame = requestAnimationFrame(measure);
        };

        const restart = () => {
            cancelAnimationFrame(frame);
            stableFrames = 0;
            frame = requestAnimationFrame(measure);
        };

        restart();
        window.addEventListener("resize", restart);
        return () => {
            cancelAnimationFrame(frame);
            window.removeEventListener("resize", restart);
        };
    }, []);

    return <div ref={ref} style={{ position: "absolute", left: offset.x, top: offset.y, width: 0, height: 0 }}>
        {children}
    </div>;
};
